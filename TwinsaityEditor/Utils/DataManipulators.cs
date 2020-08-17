using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TwinsaityEditor.Utils
{
    //Because C# UInt16 and others has no fucing interface like IParseable i have to do this. Please don't kill me.
    //Toughest of choices require strongest of will.
    public class ListManipulatorUInt16
    {
        List<UInt16> list;
        ListBox listBox;
        TextBox source;
        bool update;
        public ListManipulatorUInt16(List<UInt16> list, ListBox listBox, TextBox source, Button addBtn, Button delBtn, Button setBtn, Button upBtn, Button downBtn)
        {
            this.list = list;
            this.listBox = listBox;
            this.source = source;
            listBox.SelectedIndexChanged += UpdateSource;
            addBtn.Click += Add;
            delBtn.Click += Remove;
            setBtn.Click += Set;
            upBtn.Click += MoveUp;
            downBtn.Click += MoveDown;
            update = true;
        }
        public void UpdateSource(Object sender, EventArgs args)
        {
            if (update)
            {
                source.Text = list[listBox.SelectedIndex].ToString();
            }
        }
        public void PopulateList()
        {
            listBox.BeginUpdate();
            listBox.Items.Clear();
            foreach (UInt16 e in list)
            {
                listBox.Items.Add(e.ToString());
            }
            listBox.EndUpdate();
        }

        public void MoveUp(Object sender, EventArgs args)
        {
            if (listBox.SelectedIndex > 0)
            {
                int index = listBox.SelectedIndex;
                UInt16 val1 = list[index];
                UInt16 val2 = list[index - 1];
                list[index - 1] = val1;
                list[index] = val2;
                DisableUpdate();
                listBox.Items[index] = list[index].ToString();
                listBox.Items[index - 1] = list[index - 1].ToString();
                int top = listBox.Items.Count - 1;
                --index;
                listBox.SelectedIndex = Math.Min(Math.Max(index, 0), top);
                EnableUpdate();
            }
        }
        public void MoveDown(Object sender, EventArgs args)
        {
            if (listBox.SelectedIndex < listBox.Items.Count - 1)
            {
                int index = listBox.SelectedIndex;
                UInt16 val1 = list[index];
                UInt16 val2 = list[index + 1];
                list[index + 1] = val1;
                list[index] = val2;
                DisableUpdate();
                listBox.Items[index] = list[index].ToString();
                listBox.Items[index + 1] = list[index + 1].ToString();
                int top = listBox.Items.Count - 1;
                ++index;
                listBox.SelectedIndex = Math.Min(Math.Max(index, 0), top);
                EnableUpdate();
            }
        }
        public void Remove(Object sender, EventArgs args)
        {
            if (listBox.SelectedIndex >= 0)
            {
                int index = listBox.SelectedIndex;
                list.RemoveAt(index);
                DisableUpdate();
                listBox.Items.RemoveAt(index);
                int top = listBox.Items.Count - 1;
                listBox.SelectedIndex = Math.Min(Math.Max(index, 0), top);
                EnableUpdate();
            }
        }
        public void Add(Object sender, EventArgs args)
        {
            int index = listBox.SelectedIndex;
            if (index < 0)
            {
                index = 0;
            }
            UInt16 val;
            Boolean success;
            success = UInt16.TryParse(source.Text, out val);
            if (success)
            {
                list.Insert(index, val);
                DisableUpdate();
                listBox.Items.Insert(index, source.Text);
                listBox.SelectedIndex = index;
                EnableUpdate();
            }

        }
        public void Set(Object sender, EventArgs args)
        {
            if (listBox.SelectedIndex >= 0)
            {
                UInt16 val;
                Boolean success;
                success = UInt16.TryParse(source.Text, out val);
                if (success)
                {
                    list[listBox.SelectedIndex] = val;
                    int index = listBox.SelectedIndex;
                    DisableUpdate();
                    listBox.Items[listBox.SelectedIndex] = source.Text;
                    listBox.SelectedIndex = index;
                    EnableUpdate();
                }
            }
        }
        private void DisableUpdate()
        {
            update = false;
        }
        private void EnableUpdate()
        {
            update = true;
        }
    }

    public class ListManipulatorUInt32
    {
        List<UInt32> list;
        ListBox listBox;
        TextBox source;
        bool update;
        public ListManipulatorUInt32(List<UInt32> list, ListBox listBox, TextBox source, Button addBtn, Button delBtn, Button setBtn, Button upBtn, Button downBtn)
        {
            this.list = list;
            this.listBox = listBox;
            this.source = source;
            listBox.SelectedIndexChanged += UpdateSource;
            addBtn.Click += Add;
            delBtn.Click += Remove;
            setBtn.Click += Set;
            upBtn.Click += MoveUp;
            downBtn.Click += MoveDown;
            update = true;
        }
        public void UpdateSource(Object sender, EventArgs args)
        {
            if (update)
            {
                source.Text = list[listBox.SelectedIndex].ToString();
            }
        }
        public void PopulateList()
        {
            listBox.BeginUpdate();
            listBox.Items.Clear();
            foreach (UInt32 e in list)
            {
                listBox.Items.Add(e.ToString());
            }
            listBox.EndUpdate();
        }

        public void MoveUp(Object sender, EventArgs args)
        {
            if (listBox.SelectedIndex > 0)
            {
                int index = listBox.SelectedIndex;
                UInt32 val1 = list[index];
                UInt32 val2 = list[index - 1];
                list[index - 1] = val1;
                list[index] = val2;
                DisableUpdate();
                listBox.Items[index] = list[index].ToString();
                listBox.Items[index - 1] = list[index - 1].ToString();
                int top = listBox.Items.Count - 1;
                --index;
                listBox.SelectedIndex = Math.Min(Math.Max(index, 0), top);
                EnableUpdate();
            }
        }
        public void MoveDown(Object sender, EventArgs args)
        {
            if (listBox.SelectedIndex < listBox.Items.Count - 1)
            {
                int index = listBox.SelectedIndex;
                UInt32 val1 = list[index];
                UInt32 val2 = list[index + 1];
                list[index + 1] = val1;
                list[index] = val2;
                DisableUpdate();
                listBox.Items[index] = list[index].ToString();
                listBox.Items[index + 1] = list[index + 1].ToString();
                int top = listBox.Items.Count - 1;
                ++index;
                listBox.SelectedIndex = Math.Min(Math.Max(index, 0), top);
                EnableUpdate();
            }
        }
        public void Remove(Object sender, EventArgs args)
        {
            if (listBox.SelectedIndex >= 0)
            {
                int index = listBox.SelectedIndex;
                list.RemoveAt(index);
                DisableUpdate();
                listBox.Items.RemoveAt(index);
                int top = listBox.Items.Count - 1;
                listBox.SelectedIndex = Math.Min(Math.Max(index, 0), top);
                EnableUpdate();
            }
        }
        public void Add(Object sender, EventArgs args)
        {
            int index = listBox.SelectedIndex;
            if (index < 0)
            {
                index = 0;
            }
            UInt32 val;
            Boolean success;
            success = UInt32.TryParse(source.Text, out val);
            if (success)
            {
                list.Insert(index, val);
                DisableUpdate();
                listBox.Items.Insert(index, source.Text);
                listBox.SelectedIndex = index;
                EnableUpdate();
            }

        }
        public void Set(Object sender, EventArgs args)
        {
            if (listBox.SelectedIndex >= 0)
            {
                UInt32 val;
                Boolean success;
                success = UInt32.TryParse(source.Text, out val);
                if (success)
                {
                    list[listBox.SelectedIndex] = val;
                    int index = listBox.SelectedIndex;
                    DisableUpdate();
                    listBox.Items[listBox.SelectedIndex] = source.Text;
                    listBox.SelectedIndex = index;
                    EnableUpdate();
                }
            }
        }
        private void DisableUpdate()
        {
            update = false;
        }
        private void EnableUpdate()
        {
            update = true;
        }
    }
    public class ListManipulatorSingle
    {
        List<Single> list;
        ListBox listBox;
        TextBox source;
        bool update;
        public ListManipulatorSingle(List<Single> list, ListBox listBox, TextBox source, Button addBtn, Button delBtn, Button setBtn, Button upBtn, Button downBtn)
        {
            this.list = list;
            this.listBox = listBox;
            this.source = source;
            listBox.SelectedIndexChanged += UpdateSource;
            addBtn.Click += Add;
            delBtn.Click += Remove;
            setBtn.Click += Set;
            upBtn.Click += MoveUp;
            downBtn.Click += MoveDown;
            update = true;
        }
        public void UpdateSource(Object sender, EventArgs args)
        {
            if (update)
            {
                source.Text = list[listBox.SelectedIndex].ToString();
            }
        }
        public void PopulateList()
        {
            listBox.BeginUpdate();
            listBox.Items.Clear();
            foreach (Single e in list)
            {
                listBox.Items.Add(e.ToString());
            }
            listBox.EndUpdate();
        }

        public void MoveUp(Object sender, EventArgs args)
        {
            if (listBox.SelectedIndex > 0)
            {
                int index = listBox.SelectedIndex;
                Single val1 = list[index];
                Single val2 = list[index - 1];
                list[index - 1] = val1;
                list[index] = val2;
                DisableUpdate();
                listBox.Items[index] = list[index].ToString();
                listBox.Items[index - 1] = list[index - 1].ToString();
                int top = listBox.Items.Count - 1;
                --index;
                listBox.SelectedIndex = Math.Min(Math.Max(index, 0), top);
                EnableUpdate();
            }
        }
        public void MoveDown(Object sender, EventArgs args)
        {
            if (listBox.SelectedIndex < listBox.Items.Count - 1)
            {
                int index = listBox.SelectedIndex;
                Single val1 = list[index];
                Single val2 = list[index + 1];
                list[index + 1] = val1;
                list[index] = val2;
                DisableUpdate();
                listBox.Items[index] = list[index].ToString();
                listBox.Items[index + 1] = list[index + 1].ToString();
                int top = listBox.Items.Count - 1;
                ++index;
                listBox.SelectedIndex = Math.Min(Math.Max(index, 0), top);
                EnableUpdate();
            }
        }
        public void Remove(Object sender, EventArgs args)
        {
            if (listBox.SelectedIndex >= 0)
            {
                int index = listBox.SelectedIndex;
                list.RemoveAt(index);
                DisableUpdate();
                listBox.Items.RemoveAt(index);
                int top = listBox.Items.Count - 1;
                listBox.SelectedIndex = Math.Min(Math.Max(index, 0), top);
                EnableUpdate();
            }
        }
        public void Add(Object sender, EventArgs args)
        {
            int index = listBox.SelectedIndex;
            if (index < 0)
            {
                index = 0;
            }
            Single val;
            Boolean success;
            success = Single.TryParse(source.Text, out val);
            if (success)
            {
                list.Insert(index, val);
                DisableUpdate();
                listBox.Items.Insert(index, source.Text);
                listBox.SelectedIndex = index;
                EnableUpdate();
            }

        }
        public void Set(Object sender, EventArgs args)
        {
            if (listBox.SelectedIndex >= 0)
            {
                Single val;
                Boolean success;
                success = Single.TryParse(source.Text, out val);
                if (success)
                {
                    list[listBox.SelectedIndex] = val;
                    int index = listBox.SelectedIndex;
                    DisableUpdate();
                    listBox.Items[listBox.SelectedIndex] = source.Text;
                    listBox.SelectedIndex = index;
                    EnableUpdate();
                }
            }
        }
        private void DisableUpdate()
        {
            update = false;
        }
        private void EnableUpdate()
        {
            update = true;
        }
    }
}
