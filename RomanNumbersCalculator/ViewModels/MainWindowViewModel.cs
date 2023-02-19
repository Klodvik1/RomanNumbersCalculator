using Avalonia.Collections.Pooled;
using Avalonia.Media;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace RomanNumbersCalculator.ViewModels
{
    static class RomanNum
    {
        static Dictionary<int, string> ra = new Dictionary<int, string>{ 
            { 1000, "M" },  { 900, "CM" },  { 500, "D" },  { 400, "CD" },  { 100, "C" },
            { 90 , "XC" },  { 50 , "L" },  { 40 , "XL" },  { 10 , "X" },
            { 9  , "IX" },  { 5  , "V" },  { 4  , "IV" },  { 1  , "I" } };

        public static string ToRoman(int number) => ra.Where(d => number >= d.Key).Select(d => d.Value + ToRoman(number - d.Key)).FirstOrDefault();
        public static int ToArabic(string number) => number.Length == 0 ? 0 : ra
            .Where(d => number.StartsWith(d.Value))
            .Select(d => d.Key + ToArabic(number.Substring(d.Value.Length)))
            .First();
    }
    public class MainWindowViewModel : ViewModelBase
    {

        int LastNumber = 0;
        string Text = "";
        string operation;

        public string LogicalOperation(string operation)
        {
            if (this.operation == null) {
                this.operation = operation;
                this.LastNumber = RomanNum.ToArabic(Text);
                Text = "";
                ReactiveCommand<string, string> Textpole;
                Textpole = ReactiveCommand.Create<string, string>(str => TextPole = "");
            } else
            {
                switch (this.operation)
                {
                    case ("+"):
                        {
                            this.LastNumber += RomanNum.ToArabic(Text);
                            break;
                        }
                    case ("-"):
                        {
                            this.LastNumber -= RomanNum.ToArabic(Text);
                            break;
                        }
                    case ("*"):
                        {
                            this.LastNumber *= RomanNum.ToArabic(Text);
                            break;
                        }
                    case ("/"):
                        {
                            this.LastNumber /= RomanNum.ToArabic(Text);
                            break;
                        }
                }
                
                if (operation == "=")
                {
                    Text = RomanNum.ToRoman(this.LastNumber);
                    ReactiveCommand<string, string> Textpole;
                    Textpole = ReactiveCommand.Create<string, string>(str => TextPole = RomanNum.ToRoman(this.LastNumber));
                    this.operation = null;
                    this.LastNumber = 0;
                } else
                {
                    Text = "";
                    ReactiveCommand<string, string> Textpole;
                    Textpole = ReactiveCommand.Create<string, string>(str => TextPole = "");
                }
            }
            return Text;
        }
        public MainWindowViewModel()
        {
            InputNumber = ReactiveCommand.Create<string, string>(str => TextPole += str);
            ClearNumber = ReactiveCommand.Create<string, string>(str => TextPole = "");
        }
        public string TextPole
        {
            get => Text;
            set
            {
                this.RaiseAndSetIfChanged(ref Text, value);
            }
        }

        public ReactiveCommand<string, string> InputNumber { get; }
        public ReactiveCommand<string, string> ClearNumber { get; }
    }
}
