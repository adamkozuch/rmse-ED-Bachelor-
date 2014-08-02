﻿using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using LicencjatInformatyka_RMSE_.Additional;
using LicencjatInformatyka_RMSE_.Bases;
using LicencjatInformatyka_RMSE_.Bases.ElementsOfBases;
using LicencjatInformatyka_RMSE_.LanguageConfiguration;
using LicencjatInformatyka_RMSE_.ViewControls.AskWindows;
using LicencjatInformatyka_RMSE_.ViewControls.BrowseControls;

namespace LicencjatInformatyka_RMSE_.ViewModelFolder
{
   public class ViewModel:INotifyPropertyChanged
    {
       private IElementsNamesLanguageConfig _elementsNamesLanguageConfig;
        private readonly GatheredBases bases;
        private readonly OpenBasesActions _openBasesActions;
        private readonly ActionsOnBase _actionsOnBase;
                  
        public event PropertyChangedEventHandler PropertyChanged = null;
        public ViewModel()
        {
            

            bases = new GatheredBases(_elementsNamesLanguageConfig);

            //Instances of classes responsible for opening and actions on RMSE bases
            _openBasesActions = new OpenBasesActions(this,bases);
            _actionsOnBase = new ActionsOnBase(bases,this);


            #region RuleBaseButtons
            OpenRuleCommand = new RelayCommand(pars => _openBasesActions.ReadRuleBase()); 
            DiagnoseOutsideContradictionCommand = new RelayCommand(pars =>_actionsOnBase.CheckOutsideContradiction());
            LookRuleCommand = new RelayCommand(p => ShowWindow(new BrowseRules(this)) );
            LookAskingConditionsCommand = new RelayCommand(p =>
            {
                _actionsOnBase.FillAskingConditionsTable();
                ShowWindow(new AskingConditionsWIndow(this));
            });
            DiagnoseOutsideContradictionCommand = new RelayCommand(p => _actionsOnBase.ReportAboutOutsideContradiction());
            FlatterRuleCommand = new RelayCommand(p => _actionsOnBase.FlatterRule(_selectedRule));
            #endregion
            #region ConstrainBaseButtons
            OpenConstrainCommand = new RelayCommand(pars => _openBasesActions.ReadConstrainBase());
            ContradictionWithConstrainsCommand = new RelayCommand(pars => _actionsOnBase.CheckContradictionWithConstrains());
            LookAtBaseConstrainsCommand = new RelayCommand(p => ShowWindow(new BrowseConstrains(this)));
            #endregion
            #region ModelBaseButtons
            OpenModelCommand = new RelayCommand(pars => _openBasesActions.ReadModelBase());
          ContradictionWithModelsCommand = new RelayCommand(pars => _actionsOnBase.CheckContradictionBetweenModelsAndRulebase());
          LookAtBaseModelCommand = new RelayCommand(p => ShowWindow(new BrowseModels(this)));
            #endregion

            #region ConclusionButtons
          ConcludeCommand = new RelayCommand(pars =>  _actionsOnBase.BackwardConcludeAction(_selectedRule));
          #endregion
           ValueTrue = new RelayCommand(p => CheckedRuleVal=true);
           ValueUnknown = new RelayCommand(p => CheckedRuleVal=false);
           #region LanguageConfiguration

            MainWindowLanguageConfig =new PolishMainWindowLanguageConfig();
            ChildWindowsLanguageConfig = new PolishChildWindowsLanguageConfig();
            _elementsNamesLanguageConfig = new PolishElementsNamesLanguageConfig();

            PolishConfigurationCommand = new RelayCommand( p=>
            {
                MainWindowLanguageConfig = new PolishMainWindowLanguageConfig();
                ChildWindowsLanguageConfig = new PolishChildWindowsLanguageConfig();
                _elementsNamesLanguageConfig = new PolishElementsNamesLanguageConfig();
            });

            EnglishConfigurationCommand = new RelayCommand(p =>
            {
                MainWindowLanguageConfig = new EnglishMainWindowLanguageConfig();
                ChildWindowsLanguageConfig = new EnglishChildWindowsLanguageConfig();
                _elementsNamesLanguageConfig = new EnglishElementsLanguageConfig();
            });

            #endregion
        }

       private void ShowWindow(Window wind)
       {
           wind.Show();
       }

       // These fields are used in process of asking unknown values
        private Rule _selectedRule = new Rule();
        private bool _checkedRuleValue;
        private string _checkedRuleName;
        private Constrain _askedConstrain;
        private string _mainWindowText;

        public ICommand PolishConfigurationCommand { get; set; }
        public ICommand EnglishConfigurationCommand { get; set; }


        #region RuleBaseCommands
        public ICommand OpenRuleCommand { get; set; }
        public ICommand DiagnoseOutsideContradictionCommand { get; set; }
        public ICommand LookRuleCommand { get; set; }
        public ICommand EditRuleBaseCommand { get; set; }
        public ICommand LookAskingConditionsCommand { get; set; } //TOdo:Niezaimplemoentowane
        public ICommand CreateRuleBaseCommand { get; set; } //TODO: Niezaimplementowane
        public ICommand DiagnoseRuleRedundanciesCommand { get; set; } //TODO:Niezaimplementowane
        public ICommand FlatterRuleCommand { get; set; } //TODO:Niezaimplementowane

        #endregion
        #region ConstrainBaseCommands
        public ICommand OpenConstrainCommand { get; set; }
        public ICommand LookAtBaseConstrainsCommand { get; set; }
        public ICommand EditConstrainCommand { get; set; }
        public ICommand CreateConstrainsCommand { get; set; }
        public ICommand RedundancyConstrainCommand { get; set; }
        public ICommand ContradictionWithConstrainsCommand { get; set; }
        #endregion
        #region ModelBaseCOmmands
        public ICommand OpenModelCommand { get; set; }
        public ICommand ContradictionWithModelsCommand { get; set; }
        public ICommand LookAtBaseModelCommand { get; set; }
        public ICommand EditModelCommand { get; set; }
        public ICommand CreateModelCommand { get; set; }
        public ICommand RedundancyModelCommand { get; set; }


        #endregion

        #region AnotherCommands
        public ICommand ConcludeCommand { get; set; }

        public ICommand ValueTrue { get; set; }
        public ICommand ValueUnknown { get; set; }

       #endregion

        #region Property
        public Rule SelectedRule
        {
            get { return _selectedRule; }
            set
            {
                _selectedRule = value;
                OnPropertyChanged("SelectedRule");
            }
        }

        public bool CheckedRuleVal
        {
            get { return _checkedRuleValue; }
            set
            {
                _checkedRuleValue = value;
                OnPropertyChanged("CheckedRuleVal");
            }
        }

        public string MainWindowText
        {
            get { return _mainWindowText ; }
            set
            {
                _mainWindowText = value;
                OnPropertyChanged("MainWindowText");
            }
        }

        public Constrain AskedConstrain
        {
            get { return _askedConstrain; }
            set
            {
                _askedConstrain = value;
                OnPropertyChanged("AskedConstrain");
            }
        }
           string _valueFromConstrain;
       private string _argumentValue;
       private List<string> _askingConditionsList;
       private IMainWindowLanguageConfig _mainWindowLanguageConfig;
       private IChildWindowsLanguageConfig _childWindowsLanguageConfig;

       public string ValueFromConstrain
        {
            get
            {  
                return _valueFromConstrain;
            }
            set
            {
                _valueFromConstrain = value;
                OnPropertyChanged("ValueFromConstrain");
            }
        }

       public List<string> AskingConditionsList
       {
           get
           {
               return _askingConditionsList;
           }
           set
           {
               _askingConditionsList = value;
               OnPropertyChanged("AskingConditionsList");
           }
       }
        public string ValueArgument
        {
            get
            {
                return _argumentValue;
            }
            set
            {
                _argumentValue = value;
                OnPropertyChanged("ValueArgument");
            }
        }

        public string CheckedRuleName
        {
            get { return _checkedRuleName; }
            set
            {
                _checkedRuleName = value;
                OnPropertyChanged("CheckedRuleName");
            }
        }

        public List<Rule> RuleListProp
        {
            get { return bases.RuleBase.RulesList; }
         
        }
        public List<Constrain> ConstrainListProp
        {
            get { return bases.ConstrainBase.ConstrainList; }

        }
        public List<Model> ModelListProp
        {
            get { return bases.ModelsBase.ModelList; }

        }


       public IMainWindowLanguageConfig MainWindowLanguageConfig
       {
           get { return _mainWindowLanguageConfig; }
           set
           {
               _mainWindowLanguageConfig = value;
               OnPropertyChanged("MainWindowLanguageConfig");
           }
       }

       public IChildWindowsLanguageConfig ChildWindowsLanguageConfig
       {
           get { return _childWindowsLanguageConfig; }
           set { _childWindowsLanguageConfig = value; }
       }

       #endregion


        #region Method

     
      virtual protected void OnPropertyChanged(string propName)
      {
          if (PropertyChanged != null)
              PropertyChanged(this, new PropertyChangedEventArgs(propName));
      }
        #endregion
    }
}