﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using LicencjatInformatyka_RMSE_.Additional;
using LicencjatInformatyka_RMSE_.Bases;
using LicencjatInformatyka_RMSE_.Bases.ElementsOfBases;
using LicencjatInformatyka_RMSE_.LanguageConfiguration;
using LicencjatInformatyka_RMSE_.ViewControls.AskWindows;

namespace LicencjatInformatyka_RMSE_.ViewModelFolder
{
    public class ViewModel : INotifyPropertyChanged
    {
        public IElementsNamesLanguageConfig _elementsNamesLanguageConfig { get; set; }
        private IMainWindowLanguageConfig _mainWindowLanguageConfig;
        private IChildWindowsLanguageConfig _childWindowsLanguageConfig;
        private readonly GatheredBases bases;
        public readonly OpenBasesActions _openBasesActions;
        public readonly ActionsOnBase _actionsOnBase;
        private ButtonsLogic _buttonsLogic;
        public event PropertyChangedEventHandler PropertyChanged = null;

        public ViewModel()
        {
            #region LanguageConfiguration

            MainWindowLanguageConfig = new PolishMainWindowLanguageConfig();
            ChildWindowsLanguageConfig = new PolishChildWindowsLanguageConfig();
            _elementsNamesLanguageConfig = new PolishElementsNamesLanguageConfig();

            PolishConfigurationCommand = new RelayCommand(p =>
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


            _basesCommands = new BasesCommands(this);
            _buttonsLogic = new ButtonsLogic(this);

            #endregion

            bases = new GatheredBases(this);

            //Instances of classes responsible for opening and actions on RMSE bases
            _openBasesActions = new OpenBasesActions(this, bases);
            _actionsOnBase = new ActionsOnBase(bases, this, _elementsNamesLanguageConfig);

            #region ConclusionButtons

            ConcludeCommand = new RelayCommand(pars => _actionsOnBase.BackwardConcludeAction(_selectedRule));
            OpenBackwardConcludeWindowCommand = new RelayCommand(p=>ShowWindow(new ChooseRule(this)));
            ForwardConcludeCommand = new RelayCommand(p=> _actionsOnBase.ForwardConcludeAction());

            #endregion

            ValueTrue = new RelayCommand(p => CheckedRuleVal = true);
            ValueUnknown = new RelayCommand(p => CheckedRuleVal = false);

            StartConditionValueTrue = new RelayCommand(p=> StartConditionValue=true);
            StartConditionValueUnknown =  new RelayCommand(p => StartConditionValue=false);
            BreakButtonCommand= new RelayCommand(p=> ExceptionValue=true);
            ClearConsoleCommand = new RelayCommand(p=> MainWindowText1="");
            ArgumentNullCommand = new RelayCommand(p=> ArgumentNullMethod());
            
        }


        private bool exc;

        private string _currentRuleBasePath;

        public void ShowWindow(Window wind)
        {
            wind.Show();
        }


        public ICommand ArgumentNullCommand { get; set; }

        // These fields are used in process of asking unknown values
        private Rule _selectedRule = new Rule();
        private bool _checkedRuleValue;
        private string _checkedRuleName;
        private Constrain _askedConstrain;
        private string _mainWindowText1;
        private string _askingArgumentName;

        public ICommand PolishConfigurationCommand { get; set; }
        public ICommand EnglishConfigurationCommand { get; set; }

        public ICommand OpenBackwardConcludeWindowCommand { get; set; }
        public ICommand ForwardConcludeCommand { get; set; }

        #region AnotherCommands

        public ICommand ConcludeCommand { get; set; }
        public ICommand ClearConsoleCommand { get; set; }

        public ICommand ValueTrue { get; set; }
        public ICommand ValueUnknown { get; set; }
        public ICommand StartConditionValueTrue { get; set; }
        public ICommand StartConditionValueUnknown { get; set; }
        public ICommand BreakButtonCommand { get; set; }
        #endregion


        private string _valueFromConstrain;
        private string _argumentValue;
        private List<string> _askingConditionsList;
        private readonly BasesCommands _basesCommands;
        
        private string _startConditionName;
        private bool _startConditionValue;
        private string _mainWindowText2;

        private string _graphicPath;

       
        public string _currentModelBasePath { get; set; }
        public string _currentConstrainBasePath { get; set; }
        public string _currentGraphicBasePath { get; set; }
        public string _currentAdviceBasePath { get; set; }
        public string _currentSoundBasePath { get; set; }


        public string _currentGraphicFolderPath { get; set; }
        public string _currentAdviceFolderPath { get; set; }
        public string _currentSoundFolderPath { get; set; }

        public string advicePath { get; set; }
        public string soundPath { get; set; }

        public string adviceText { get; set; }


        #region Property

        public string StartConditionName
        {
            get { return _startConditionName; }
            set
            {
                _startConditionName = value;
                OnPropertyChanged("StartConditionName");
            }
        }

        public string GraphicPath
        {
            get { return _graphicPath; }
            set
            {
                _graphicPath = value;
                OnPropertyChanged("GraphicPath");
            }
        }

        public string CurrentRuleBasePath
        {
            get { return _currentRuleBasePath; }
            set
            {
                _currentRuleBasePath = value;
             
            }
        }



        public bool StartConditionValue
        {
            get { return _startConditionValue; }
            set
            {
                _startConditionValue = value;
                OnPropertyChanged("StartConditionValue");
            }
        }

        public ButtonsLogic ButtonsLogic
        {
            get { return _buttonsLogic; }
            set
            {
                _buttonsLogic = value;
                OnPropertyChanged("ButtonsLogic");
            }
        }


        public string AskingArgumentName
        {
            get { return _askingArgumentName; }
            set
            {
                _askingArgumentName = value;
                OnPropertyChanged("AskingArgumentName");
            }
        }


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

        public string MainWindowText1
        {
            get { return _mainWindowText1; }
            set
            {
                _mainWindowText1 = value;
                OnPropertyChanged("MainWindowText1");
            }
        }

        public string MainWindowText2
        {
            get { return _mainWindowText2; }
            set
            {
                _mainWindowText2 = value;
                OnPropertyChanged("MainWindowText2");
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

   

        public string ValueFromConstrain
        {
            get { return _valueFromConstrain; }
            set
            {
                _valueFromConstrain = value;
                OnPropertyChanged("ValueFromConstrain");
            }
        }

        public List<string> AskingConditionsList
        {
            get { return _askingConditionsList; }
            set
            {
                _askingConditionsList = value;
                OnPropertyChanged("AskingConditionsList");
            }
        }

        public string ValueArgument
        {
            get { return _argumentValue; }
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

        public BasesCommands BasesCommandsProperty
        {
            get { return _basesCommands; }
        }

        public bool ExceptionValue
        {
            get { return exc; }
            set { exc = value; }
        }

        #endregion

        #region Method

        protected virtual void OnPropertyChanged(string propName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }

        public void AskingRuleValueMethod(SimpleTree simpleTree)
        {
            CheckedRuleName = simpleTree.rule.Conclusion;
            AskRuleValue window = new AskRuleValue(this);

            window.ShowDialog();
             if(exc)
                 throw new ApplicationException();// TODO:Może wystapić bug związany z zamkniciem okna x w lewym górnym rogu

            simpleTree.ConclusionValue = CheckedRuleVal;
        }

        public string AskingArgumentValueMethod(string argument)
        {
            AskingArgumentName = argument;
            ValueArgument = "";
            AskArgument window = new AskArgument(this);
            window.ShowDialog();
            if(exc)
                throw new ApplicationException();
            if(ValueArgument!=null)
            { 
            bases.ModelsBase.ArgumentList.Add(new Argument()
            {
                ArgumentName = AskingArgumentName,
                Value = ValueArgument
            });
             }
            return ValueArgument;
        }


        public void ArgumentNullMethod()
        {
            ValueArgument = null;
        }


        public bool AskingStartConditionValue(string startCondition)
        {
            StartConditionName = startCondition;
            var window = new AskStartCondition(this);

            window.ShowDialog();

            bases.ModelsBase.ModelFactList.Add(new Fact()
            {
                FactName = StartConditionName,
                FactValue = StartConditionValue
            }); 
            return StartConditionValue;
        }


        public void AskingForwardRuleValueMethod(string conclusion)
        {
            CheckedRuleName = conclusion;
            AskRuleValue window = new AskRuleValue(this);

            window.ShowDialog(); // TODO:Może wystapić bug związany z zamkniciem okna x w lewym górnym rogu
            if (exc)
                throw new ApplicationException();
           
               bases.FactBase.FactList.Add(new Fact(){FactName = CheckedRuleName, FactValue = CheckedRuleVal});  
            // TODO:Może wystapić bug związany z zamkniciem okna x w lewym górnym rogu
            
        }


        public void AskingConstrainValueMethod(Constrain conclusion)
        {
            
            var window = new AskConstrain(this);

            window.ShowDialog(); // TODO:Może wystapić bug związany z zamkniciem okna x w lewym górnym rogu

            bases.FactBase.FactList.Add(new Fact() { FactName = CheckedRuleName, FactValue = CheckedRuleVal });
        }


        #endregion
    }
}