﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using LicencjatInformatyka_RMSE_.Additional;
using LicencjatInformatyka_RMSE_.Bases.ElementsOfBases;
using LicencjatInformatyka_RMSE_.OperationsOnBases;
using LicencjatInformatyka_RMSE_.ViewModelFolder;

namespace LicencjatInformatyka_RMSE_.Bases
{
    public class ModelBase
    {
        private readonly ViewModel _config;
        private List<Model> _modelList = new List<Model>();
        private List<Argument> _argumentList = new List<Argument>();
        private  List<Fact>  _modelFactsList = new List<Fact>();

        public ModelBase(ViewModel config)
        {
            _config = config;
        }


        public List<Model> ModelList
        {
            get { return _modelList; }
            set { _modelList = value; }
        }


        public List<Fact> ModelFactList
        {
            get { return _modelFactsList; }
            set { _modelFactsList = value; }
        }

        public List<Argument> ArgumentList
        {
            get { return _argumentList; }
            set { _argumentList = value; }
        }

 

        public void ReadModels(string models)
        {
            foreach (string line in File.ReadLines(models, Encoding.GetEncoding("Windows-1250")))
            {
                RuleChecker(line);
            }
        }

        private void RuleChecker(string line)
        {
            int modelNameEnd = line.IndexOf("(");
            string TypeOfModel = line.Substring(0, modelNameEnd);

            //browsking apriopriate model
            // Trzeba się zastanowic nad leszym sposobem na identyfikację mało uniwersalny
            if (TypeOfModel == _config._elementsNamesLanguageConfig.SimpleModel)
            {
                ModelList.Add(SimpleModel(line));
            }
            else if (TypeOfModel == _config._elementsNamesLanguageConfig.ExtendedModel)
            {
                ModelList.Add(ExtendedModel(line));
            }
            else if (TypeOfModel == _config._elementsNamesLanguageConfig.LinearModel)
            {
                ModelList.Add(LinearModel(line));
            }
            else if (TypeOfModel == _config._elementsNamesLanguageConfig.PolyModel)
            {
                ModelList.Add(PolyModel(line));
            }
            else if (TypeOfModel == _config._elementsNamesLanguageConfig.Argument)
            {
                ArgumentList.Add(ReturnArgument(line));
            }
            else if (TypeOfModel == _config._elementsNamesLanguageConfig.ModelFact)
            {
                _modelFactsList.Add(ModelFact(line));
            }

            
            
        }

        public Model SimpleModel(string line)
        {
            line = OperationsOnString.RemoveBeggining(line);
            List<string> result = OperationsOnString.SplitArguments(line);

            int semaphorNumber = int.Parse(result.Last());

            bool semaphorValue = semaphorNumber == 1;

            return new Model(int.Parse(result[0]), result[1], result[2], result[3],
                result[4], result[5], semaphorValue);
        }

        private Model ExtendedModel(string line)
        {
            line = OperationsOnString.RemoveBeggining(line);
            string[] tableStrings = OperationsOnString.SplitRuleToTwoPartsConditionsAndAnother(line);
            List<string> result = OperationsOnString.SplitArguments(tableStrings[0]);
            List<string> argumentsList = OperationsOnString.SplitArguments(tableStrings[1]);
            string sign="";
            int semaphoreNumber = int.Parse(result.Last());
            bool semaphoreValue = semaphoreNumber == 1;
            if (result[3].ToLowerInvariant().Contains('>') || result[3].ToLowerInvariant().Contains('<'))  //todo:rozwiązanie działa ale do poprawienia
            {
                if (result[3].Count() < 3)
                {
                    sign = result[3] + "," + result[4];
                }
                else
                    sign = result[3];
            }
            else
            {
                sign = result[3];
            }
            // nie cały znak
            return new Model(int.Parse(result[0]), result[1], result[2], sign, argumentsList, semaphoreValue);
        }

        private Model LinearModel(string line)
        {
            line = OperationsOnString.RemoveBeggining(line);
            string[] table1 = OperationsOnString.SplitRuleToTwoPartsConditionsAndAnother(line);
            string[] table2 = OperationsOnString.SplitRuleToTwoPartsConditionsAndAnother(table1[0]);


            List<string> factorsList = OperationsOnString.SplitArguments(table1[1]);
            List<string> result = OperationsOnString.SplitArguments(table2[0]);
            List<string> variablesList = OperationsOnString.SplitArgumentsForModel(table2[1]);

            int semaphorNumber = int.Parse(result.Last());
            bool semaphorValue = semaphorNumber == 1;

            return new Model(int.Parse(result[0]), result[1], result[2], factorsList, variablesList, semaphorValue);
        }

        private Model PolyModel(string line)
        {
            line = OperationsOnString.RemoveBeggining(line);
            string[] table1 = OperationsOnString.SplitRuleToTwoPartsConditionsAndAnother(line);
            string[] table2 = OperationsOnString.SplitRuleToTwoPartsConditionsAndAnother(line);
            List<string> factorsList = OperationsOnString.SplitArguments(table1[1]);
            List<string> result = OperationsOnString.SplitArguments(table2[0]);
            char[] commaSeparator = new char[] { ',' };

            
            List<string> powerList = table2[1].Split(commaSeparator).ToList();
            List<int> powerList1 = new List<int>();
            foreach (var VARIABLE in powerList)
            {
                powerList1.Add(int.Parse(VARIABLE));
            }

            int semaphorNumber = int.Parse(result.Last());
            bool semaphorValue = semaphorNumber == 1;

            return new Model(int.Parse(result[0]), result[1], result[2], result[3], factorsList, powerList1,
                semaphorValue);
        }


        private Fact ModelFact(string line)
        {
            line = OperationsOnString.RemoveBeggining(line);
            List<string> list = OperationsOnString.SplitArguments(line);
            return new Fact(){FactName = list[0],FactValue = true};
        }

        private Argument ReturnArgument(string line)
        {
            line = OperationsOnString.RemoveBeggining(line);
            List<string> list = OperationsOnString.SplitArguments(line);
            return new Argument(list[0], list[1]);
        }


     
    }
}