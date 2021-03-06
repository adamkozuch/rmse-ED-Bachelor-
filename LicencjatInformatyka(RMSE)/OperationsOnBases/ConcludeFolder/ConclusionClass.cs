using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using LicencjatInformatyka_RMSE_.Additional;
using LicencjatInformatyka_RMSE_.Bases;
using LicencjatInformatyka_RMSE_.Bases.ElementsOfBases;
using LicencjatInformatyka_RMSE_.ViewControls;
using LicencjatInformatyka_RMSE_.ViewControls.AskWindows;
using LicencjatInformatyka_RMSE_.ViewModelFolder;

namespace LicencjatInformatyka_RMSE_.OperationsOnBases.ConcludeFolder
{
    /// <summary>
    /// Class ConclusionClass.
    /// </summary>
    public class ConclusionClass
    {

        /// <summary>
        /// The _bases
        /// </summary>
        private readonly GatheredBases _bases;
        /// <summary>
        /// The _view model
        /// </summary>
        private readonly ViewModel _viewModel;

        private readonly IElementsNamesLanguageConfig _config;

        /// <summary>
        /// The _constrain actions
        /// </summary>
        private readonly ConstrainActions _constrainActions;
        /// <summary>
        /// The _model actions
        /// </summary>
        private readonly ModelActions _modelActions;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConclusionClass"/> class.
        /// </summary>
        /// <param name="bases">The bases.</param>
        /// <param name="viewModel">The view model.</param>
        public ConclusionClass(GatheredBases bases, ViewModel viewModel, IElementsNamesLanguageConfig config)
        {
            _bases = bases;
            _viewModel = viewModel;
            _config = config;
            _constrainActions = new ConstrainActions(this, _viewModel, bases);
            _modelActions = new ModelActions(this,_viewModel,bases,config);
        }

        /// <summary>
        /// Askeds the conditions.
        /// </summary>
         public   int AskedConditions()
       {
           var askingConditionList= new List<string>();
            foreach (var rule in _bases.RuleBase.RulesList)
            {
                foreach (var condition in rule.Conditions)
                {
                    if (_bases.RuleBase.RulesList.Any(p => p.Conclusion == condition))
                    {
                       
                    }
                    else
                    {
                        if (CheckIfStringIsFact(condition, _bases.FactBase.FactList) == false)
                            //if()
                        {
                            foreach (var element in askingConditionList)
                            {
                                if (condition == element)
                                    goto label;
                            }
                            askingConditionList.Add(condition);
                        label:;
                        }
                    }
                }
                
            }
                 _viewModel.AskingConditionsList = askingConditionList;
            return askingConditionList.Count;

       }

         /// <summary>
         /// Flatters the rule.
         /// </summary>
         /// <param name="flatteredRule">The flattered rule.</param>
        public string FlatterRule(Rule flatteredRule)
        {
            var differenceList= new List<List<Rule>>();
            var tree = TreeOperations.ReturnComplexTreeAndDifferences(_bases, flatteredRule,out differenceList);
            var possibleTrees = TreeOperations.ReturnPossibleTrees(tree, differenceList);
             int i = 0;
             string s = "";
            foreach (var possibleTree in possibleTrees)
            {
             // _viewModel.MainWindowText1+=  
                s+=  GetFlatteredRuleDescription(possibleTree);
                i++;
            }
             return s;

        }

        public static string GetFlatteredRuleDescription(List<SimpleTree> possibleTree)
        {
            var flatter = possibleTree.Where(p => p.Askable == true);
            var root = possibleTree.Where(p => p.Parent == null);
            int i = 0;
            string descriptionString="";
            foreach (var simpleTree in flatter)
            {
                if (i == 0)
                {
                    descriptionString += "Sp�aszczenie dla regu�y :" + root.First().rule.Conclusion + "\n";
                    i++;
                }

                descriptionString += simpleTree.rule.Conclusion + " ";
            }
            flatter = possibleTree.Where(p => p.Askable == false);
            descriptionString += "\n";
          
            foreach (var simpleTree in flatter)
            {
                if (simpleTree.rule.NumberOfRule == root.First().rule.NumberOfRule)
                {
                    descriptionString += "U�yte zosta�y nast�puj�ce regu�y " + "\n";
                }
                else
                    descriptionString += simpleTree.rule.NumberOfRule + " ";
            }
            descriptionString += "\n\n\n";

            return descriptionString;
        }


        /// <summary>
        /// Backwards the conclude.
        /// </summary>
        /// <param name="checkedRule">The checked rule.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public bool BackwardConclude( Rule checkedRule)
        {
            //FindHelpfulAssets(checkedRule);
            var differenceList = new List<List<Rule>>();
            var tree = TreeOperations.ReturnComplexTreeAndDifferences(_bases, checkedRule,out differenceList);

            var possibleTrees = TreeOperations.ReturnPossibleTrees(tree, differenceList);

            try
            {
                foreach (var onePossibility in possibleTrees) //flattering all possible configurations of conditions
                {
                    List<SimpleTree> askableTable = onePossibility.Where(var => var.Askable).ToList();
                    //If askable simple tree can be a model,condition of rule or condition of constrain

                    // sprawdzamy czy jest w bazie faktow
                    foreach (SimpleTree simpleTree in askableTable)
                    {
                        if (CheckIfStringIsFact(simpleTree.rule.Conclusion, _bases.FactBase.FactList))
                            simpleTree.ConclusionValue = true;
                    }

                    bool conclusionValue = CheckConclusionValueOrCountModel(askableTable);
                   

                    if (conclusionValue)
                    {

                        MessageBox.Show("Hipoteza :"+checkedRule.Conclusion+" prawdziwa");
                        ShowAdviceGraphicOrSound(checkedRule);
                        CleanBase();
                        //TODO : trzeba wyzerowa� bazy
                        return true;
                    }
                }
                MessageBox.Show("Nie uda�o si� potwierdzi� wniosku regu�y "+checkedRule.NumberOfRule.ToString()+"\n"+ 
                    "Ze wzgl�du na za�o�enie otwatrego �wiata hipoteza niepotwierdzona");
                CleanBase();
                return false;
                //TODO : trzeba wyzerowa� bazy
            }
            catch (ApplicationException  ex)
            {
               CleanBase();
                MessageBox.Show("Wnioskowanie przerwane");
                _viewModel.ExceptionValue = false;
            }
            return false;
        }

        private void CleanBase()
        {
            _bases.FactBase.FactList = new List<Fact>();
            _bases.ModelsBase.ModelFactList = new List<Fact>();
            _bases.ArgumentBase.argumentList = new List<Argument>();
        }

        private void ShowAdviceGraphicOrSound(Rule checkedRule)
        {
            int number = checkedRule.NumberOfRule;
            foreach (var advice in _bases.AdviceBase.AdviceList)
            {
                if (advice.adviceNumber == number)
                {
                    AskShowAdvice(advice);
                }
            }

                foreach (var graphic in _bases.GraphicBase.GraphicList)
            {
                if (graphic.graphicNumber == number)
                {
                    AskShowGraphic(graphic);
                }
            }

                    foreach (var sound in _bases.SoundBase.SoundList)
            {
                if (sound.soundNumber == number)
                {
                    AskShowSound(sound);
                }
            }


        }

        private void AskShowSound(Sound sound)
        {
            
        }

        private void AskShowGraphic(Graphic graphic)
        {
            if (MessageBox.Show("Rada", "Do wniosku za��czona jest grafika czy chcesz j� zobaczy�?",
                MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                _viewModel.GraphicPath = _viewModel._currentGraphicFolderPath + "\\" + graphic.graphicPath;
                GraphicWindow window = new GraphicWindow(_viewModel);
                window.Show();
            }
        }

        private void AskShowAdvice(Advice advice)
        {
            if (MessageBox.Show("Rada", "Do wniosku za��czona jest rada czy chcesz j� zobaczy�?", 
                MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
               
                _viewModel.advicePath = _viewModel._currentAdviceFolderPath + "\\" + advice.advicePath;

            foreach (string line in File.ReadLines(_viewModel.advicePath, Encoding.GetEncoding("Windows-1250")))
            {

                _viewModel.adviceText += line + "\n";

            }

            AdviceWindow window = new AdviceWindow(_viewModel);
            window.Show();
            }
           
           
        }

        public void FindHelpfulAssets(Rule checkedRule)
        {
            List<Graphic> findedGraphics = _bases.GraphicBase.GraphicList.Where(p => p.graphicNumber == checkedRule.NumberOfRule).ToList();

            FindAdvice(checkedRule.NumberOfRule);
            if(findedGraphics.Count!=0)
            FindGraphic(checkedRule.NumberOfRule);
            FindSound(checkedRule.NumberOfRule);

        }

        private void FindSound(int numberOfRule)
        {
            throw new NotImplementedException();
        }

        private void FindGraphic(int numberOfRule)
        {
            List<Graphic> findedGraphics = _bases.GraphicBase.GraphicList.Where(p => p.graphicNumber == numberOfRule).ToList();
            if(findedGraphics.Count!=0)
                AskShowGraphic(findedGraphics.First());

        }

        private void FindAdvice(int numberOfRule)
        {
            List<Advice> findedGraphics =
                _bases.AdviceBase.AdviceList.Where(p => p.adviceNumber == numberOfRule).ToList();
            if (findedGraphics.Count != 0)
                AskShowAdvice(findedGraphics.First());
        }

        /// <summary>
        /// Checks the conclusion value or count model.
        /// </summary>
        /// <param name="askingTable">The asking table.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        private bool CheckConclusionValueOrCountModel(List<SimpleTree> askingTable)
        {
            int i = 0;
            foreach (SimpleTree simpleTree in askingTable)
            {
              
                _constrainActions.ConstrainAsk(simpleTree);
                if (simpleTree.Model)
                {
                    var checker=   CheckIfStringIsFact(simpleTree.rule.Conclusion, _bases.FactBase.FactList);

                    if (checker==false) 
                    {
                        var value  = _modelActions.ProcessModel(simpleTree.rule.Conclusion);

                        if (value == null)
                        {
                            MessageBox.Show("Brak danych do ukonkretnienia modelu " + simpleTree.rule.Conclusion);
                            _bases.FactBase.FactList.Add(new Fact()
                            {
                                FactName = simpleTree.rule.Conclusion,
                                FactValue = false
                            });

                        }
                        else
                        {
                            _bases.FactBase.FactList.Add(new Fact()
                            {
                                FactName = simpleTree.rule.Conclusion,
                                FactValue = (bool)value
                            });
                            if ((bool) value)
                            {
                                i++;
                                IfParentTrueWrite(simpleTree);
                            }
                            else
                            {
                                break;
                            }
                            //TODO: trzeba sprawdzi� czy warunek jest faktem
                            simpleTree.ConclusionValue = (bool)value;
                            
                        }
                    }
                }
                else
                {
                    if (CheckIfStringIsFact(simpleTree.rule.Conclusion, _bases.FactBase.FactList))
                    {
                        i++;
                        simpleTree.ConclusionValue = true;
                        IfParentTrueWrite(simpleTree);
                    }

                    else
                    {
                        _viewModel.AskingRuleValueMethod(simpleTree);


                        if (_viewModel.CheckedRuleVal)
                        {
                            i++;
                            IfParentTrueWrite(simpleTree); // Check if parent changed value to concrete

                        }
                    }
                }
            }

            if (i == askingTable.Count)
                return true; // hipoteza jest prawdziwa
            return false; //else trzeba sprawdzac dalej
        }

        private void IfParentTrueWrite(SimpleTree simpleTree)
        {
            if (simpleTree.Askable)
            {
                _viewModel.MainWindowText1 += simpleTree.rule.Conclusion + " Fakt \n";

            }
            int i = 0;
            if (simpleTree.Parent != null)
            {
                foreach (var simple in simpleTree.Parent.Children)
                {
                    if (simple.ConclusionValue)
                        i++;
                }

                if (i == simpleTree.Parent.Children.Count)
                {
                    _viewModel.MainWindowText2 += simpleTree.Parent.rule.Conclusion + " Wynik \n";
                    IfParentTrueWrite(simpleTree.Parent);
                }
            }

            
        }


        /// <summary>
        /// Finds the conditions or return null.
        /// </summary>
        /// <param name="checkedCondition">The checked condition.</param>
        /// <param name="baseList">The base list.</param>
        /// <returns>List&lt;System.String&gt;.</returns>
        public static
            List<string> FindConditionsOrReturnNull
            (string checkedCondition, List<Rule> baseList)
        {
            var lista = new List<string>();

            foreach (Rule rule in baseList)
            {
                if (rule.Conclusion == checkedCondition) // Checking if rule in rulebase is condition 
                {
                    lista.AddRange(rule.Conditions); //LINQ
                    // zwraca dowoln� liczb� zestaw�w warunkow( jakby by�y np. dwie reguly o tej samej nazwie)
                }
            }
            if (lista.Count == 0) // If not find conditions for rule return checked condition 
                return null; //    lista.Add(checkedCondition);

            return lista;
        }


        /// <summary>
        /// Finds the rules with particular conclusion.
        /// </summary>
        /// <param name="NameOfCondition">The name of condition.</param>
        /// <param name="baseList">The base list.</param>
        /// <returns>List&lt;Rule&gt;.</returns>
        public static List<Rule> FindRulesWithParticularConclusion
            (string NameOfCondition, List<Rule> baseList)
        {
            var rulesThatMatch = new List<Rule>();

            foreach (Rule rule in baseList)
            {
                if (rule.Conclusion == NameOfCondition)
                {
                    rulesThatMatch.Add(rule);
                }
            }
            //If return empty list condition for ask (dopytywalny)
            return rulesThatMatch;
        }




        /// <summary>
        /// Checks if string is fact.
        /// </summary>
        /// <param name="nameOfConclusion">The name of conclusion.</param>
        /// <param name="listOfFacts">The list of facts.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public static bool CheckIfStringIsFact(string nameOfConclusion, List<Fact> listOfFacts)
        {
            foreach (Fact factItem in listOfFacts)
            {
                if (factItem.FactName == nameOfConclusion)
                {
                    return true;
                }
            }
            return false;
        }
    }
    
    }
