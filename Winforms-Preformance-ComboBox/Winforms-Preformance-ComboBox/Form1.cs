using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;
using Trevor.Fellman;

namespace Winforms_Preformance_ComboBox
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

            var l = new TextBoxTraceListener(this.textBox1);
            Debug.Listeners.Add(l);

            Debug.WriteLine(Debugger.IsAttached ? "Debugger Attached" : "No Debugger");


            List<object> data = new List<object>();
            for (int i = 0; i < 25000; i++)
                data.Add(i);

            Stopwatch sw = new Stopwatch();




            sw.Start();
            AddComboboxItemsForEach(comboBox1, data);
            sw.Stop();
            Debug.WriteLine("AddComboboxItemsForEach took {0}ms", sw.ElapsedMilliseconds);

            sw.Restart();
            AddComboboxItemsBeginUpdate(comboBox2, data);
            sw.Stop();
            Debug.WriteLine("AddComboboxItemsBeginUpdate took {0}ms", sw.ElapsedMilliseconds);

            sw.Restart();
            AddComboboxItemsAddRange(comboBox3, data);
            sw.Stop();
            Debug.WriteLine("AddComboboxItemsAddRange took {0}ms", sw.ElapsedMilliseconds);

            sw.Restart();
            AddComboboxDataSource(comboBox4, data);
            sw.Stop();
            Debug.WriteLine("AddComboboxDataSource took {0}ms", sw.ElapsedMilliseconds);






            //Debugger Attached
            //AddComboboxItemsForEach took 6082ms
            //AddComboboxItemsBeginUpdate took 1221ms
            //AddComboboxItemsAddRange took 1312ms
            //AddComboboxDataSource took 1441ms

            //Adding one at a time has extra overhead, try any of the latter 3 techniques

        }

        void AddComboboxItemsForEach(ComboBox cb, List<object> l)
        {
            foreach (object i in l)
                cb.Items.Add(i);

            //This is common, but not the most performant way to add Items to a Combobox or ist box
        }

        void AddComboboxItemsBeginUpdate(ComboBox cb, List<object> l)
        {
            cb.BeginUpdate();
            foreach (object i in l)
                cb.Items.Add(i);
            cb.EndUpdate();

            //https://docs.microsoft.com/en-us/dotnet/api/system.windows.forms.combobox.beginupdate?view=net-5.0
            //This method (BeginUpdate) prevents the control from painting until the EndUpdate method is called.

            // The preferred way to add items to the ComboBox is to use the AddRange method of the
            // ComboBox.ObjectCollection class (through the Items property of the ComboBox).

            // This enables you to add an array of items to the list at one time.However,
            // if you want to add items one at a time using the Add method of the ComboBox.ObjectCollection class,
            // you can use the BeginUpdate method to prevent the control from repainting the ComboBox each time
            // an item is added to the list.
            // Once you have completed the task of adding items to the list,
            // call the EndUpdate method to enable the ComboBox to repaint.
            // This way of adding items can prevent flicker during the drawing of the ComboBox when
            // a large number of items are being added to the list.

        }

        void AddComboboxItemsAddRange(ComboBox cb, List<object> l)
        {
            cb.Items.AddRange(l.ToArray());

            //https://docs.microsoft.com/en-us/dotnet/api/system.windows.forms.combobox.objectcollection.addrange?view=net-5.0#System_Windows_Forms_ComboBox_ObjectCollection_AddRange_System_Object___
            // When using this method to add items to the collection, you do not need to call the BeginUpdate and EndUpdate methods to optimize performance.
        }

        void AddComboboxDataSource(ComboBox cb, List<object> l)
        {
            cb.DataSource=l;

            //https://docs.microsoft.com/en-us/dotnet/api/system.windows.forms.combobox.datasource?view=net-5.0
        }
    }
}
