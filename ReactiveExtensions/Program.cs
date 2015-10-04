using System;
using System.Reactive.Linq;
using System.Windows.Forms;

namespace ReactiveExtensions
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            // ObservableGetValue();
            GetTextBoxValue();
        }

        private static void GetTextBoxValue()
        {
            var textbox = new TextBox();
            var form = new Form
            {
                Controls = {textbox}
            };

            //Using Rx, to get the textchanged 

            var textChanged = Observable.FromEventPattern<EventArgs>(textbox, "TextChanged");

            //then, I will subscribe the same using LINQ Projection

            var query = from e in textChanged
                select ((TextBox) e.Sender).Text;

            //Hence, if the application quits, i want to unsubscribe
            using (query.Subscribe(Console.WriteLine))
            {
                Application.Run(form);
            }
        }


        private static void ObservableGetValue()
        {
            IObservable<string> obj = Observable.Generate(
                0, //Sets the initial value like for loop
                _ => true, //Don't stop till i say so, infinite loop
                i => i + 1, //Increment the counter by 1 everytime
                i => new string('#', i), //Append #
                i => TimeSelector(i)); //delegated this to private method which just calculates time

            //Subscribe here
            using (obj.Subscribe(Console.WriteLine))
            {
                Console.WriteLine("Press any key to exit!!!");
                Console.ReadLine();
            }
        }

        //Returns TimeSelector
        private static TimeSpan TimeSelector(int i)
        {
            return TimeSpan.FromSeconds(i);
        }
    }
}