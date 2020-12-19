using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using ReactiveUI;

namespace a4.ViewModels
{
    public class ViewModelBase : ReactiveObject
    {
        private string tLog;
        public string TLog
        {
            get => tLog;
            set => this.RaiseAndSetIfChanged(ref tLog, value);
        }

        private string funcInfo;
        public string FuncInfo
        {
            get => funcInfo;
            set => this.RaiseAndSetIfChanged(ref funcInfo, value);
        }

        private string resultText;
        public string ResultText
        {
            get => resultText;
            set => this.RaiseAndSetIfChanged(ref resultText, value);
        }

        private string[] availableFunc;
        public string[] AvailableFunc 
        { 
            get=>availableFunc; 
            set=>this.RaiseAndSetIfChanged(ref availableFunc, value); 
        }

        private string selectedFunc;
        public string SelectedFunc
        {
            get => selectedFunc;
            set => this.RaiseAndSetIfChanged(ref selectedFunc, value);
        }

        private string axe1name;
        public string Axe1Name
        {
            get => axe1name;
            set => this.RaiseAndSetIfChanged(ref axe1name, value);
        }

        private string axe2name;
        public string Axe2Name
        {
            get => axe2name;
            set => this.RaiseAndSetIfChanged(ref axe2name, value);
        }
    }
}
