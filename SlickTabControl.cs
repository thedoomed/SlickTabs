using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

public delegate void TOnSelect(object current, object last);

public class SlickTabControl
{
    public Form Parent;
    public int X;
    public int Y;
    public int MaxTabs;
    public string DefaultCaption;
    public int SelectedIndex;
    public SlickTab SelectedItem;
    public Color TabBackColor;
    public Color TabSelectedColor;

    public event TOnSelect OnSelect;

    private Image TabBackgroundImage;
    private List<SlickTab> Tabs;
    private Label btn_open;

    private void initOpenButton()
    {
        btn_open = new Label();
        btn_open.Text = "+";
        btn_open.Font = new Font("Arial", 14.0f);
        btn_open.ForeColor = Color.Gray;
        btn_open.BackColor = Color.Transparent;
        btn_open.MouseEnter += (o, i) => { btn_open.ForeColor = Color.DarkGray; };
        btn_open.MouseLeave += (o, i) => { btn_open.ForeColor = Color.Gray; };
        btn_open.MouseUp += (o, i) =>
        {
            if (Tabs.Count < MaxTabs)
            {
                SlickTab LastSelectedItem = SelectedItem;

                Add(DefaultCaption);
                SelectedIndex = Tabs.Count - 1;
                SelectedItem = Tabs[SelectedIndex];
                Update();

                if (OnSelect != null)
                {
                    OnSelect.DynamicInvoke(SelectedItem, LastSelectedItem);
                }
            }
        };
        Parent.Controls.Add(btn_open);
    }


    public SlickTabControl(Form parentForm)
    {
        Parent = parentForm;
        X = 0;
        Y = 0;
        TabBackgroundImage = null;
        TabBackColor = Color.FromArgb(0x3C, 0x41, 0x42);
        TabSelectedColor = Color.FromArgb(0x28, 0x28, 0x32);
        Tabs = new List<SlickTab>();
        MaxTabs = 3;
        DefaultCaption = "Untitled";

        initOpenButton();
    }

    public SlickTabControl(Form parentForm, int xpos, int ypos)
    {
        Parent = parentForm;
        X = xpos;
        Y = ypos;
        TabBackgroundImage = null;
        TabBackColor = Color.FromArgb(0x3C, 0x41, 0x42);
        TabSelectedColor = Color.FromArgb(0x28, 0x28, 0x32);
        Tabs = new List<SlickTab>();
        MaxTabs = 3;
        DefaultCaption = "Untitled";

        initOpenButton();
    }

    public void SetPosition(int xpos, int ypos)
    {
        X = xpos;
        Y = ypos;
    }

    public void SetTabBackgroundImage(Image img)
    {
        TabBackgroundImage = img;
    }

    private SlickTab CreateBlankTab()
    {
        SlickTab slicktab = new SlickTab();

        slicktab.btn_x.Font = new Font("Arial", 9.75f, FontStyle.Regular);
        slicktab.btn_x.Text = "x";
        slicktab.btn_x.Size = new Size(18, 18);
        slicktab.btn_x.ForeColor = Color.White;

        slicktab.btn_x.MouseUp += (o, i) => { if (Tabs.Count > 1) { Remove(slicktab); Update(); } };
        slicktab.btn_x.MouseEnter += (o, i) => { slicktab.btn_x.ForeColor = Color.Gray; };
        slicktab.btn_x.MouseLeave += (o, i) => { slicktab.btn_x.ForeColor = Color.White; };

        slicktab.lbl_caption.Font = new Font("Arial", 9.0f, FontStyle.Regular);
        slicktab.lbl_caption.ForeColor = Color.White;
        slicktab.lbl_caption.Size = new Size(80, 18);
        
        slicktab.SelectedColor = TabSelectedColor;
        slicktab.BackColor = TabBackColor;

        Parent.Controls.Add(slicktab.tab_bg);
        Parent.Controls.Add(slicktab.btn_x);
        Parent.Controls.Add(slicktab.lbl_caption);

        int current_index = Tabs.Count;
        if (current_index == 0)
        {
            SlickTab LastSelectedItem = SelectedItem;

            SelectedItem = slicktab;
            SelectedIndex = 0;

            if (OnSelect != null)
            {
                OnSelect.DynamicInvoke(slicktab, LastSelectedItem);
            }
        }

        slicktab.lbl_caption.BringToFront();
        slicktab.btn_x.BringToFront();
        slicktab.Update(X, Y);

        Tabs.Add(slicktab);
        
        slicktab.lbl_caption.MouseUp += (o, i) =>
        {
            if (SelectedItem != slicktab)
            {
                SlickTab LastSelectedItem = SelectedItem;

                SelectedIndex = current_index;
                SelectedItem = slicktab;
                Update();

                if (OnSelect != null)
                {
                    OnSelect.DynamicInvoke(slicktab, LastSelectedItem);
                }
            }
        };

        slicktab.tab_bg.MouseUp += (o, i) =>
        {
            if (SelectedItem != slicktab)
            {
                SlickTab LastSelectedItem = SelectedItem;

                SelectedIndex = current_index;
                SelectedItem = slicktab;
                Update();

                if (OnSelect != null)
                {
                    OnSelect.DynamicInvoke(slicktab, LastSelectedItem);
                }
            }
        };

        return slicktab;
    }

    public SlickTab Add()
    {
        SlickTab tab = CreateBlankTab();
        tab.Caption = "Untitled";
        tab.Data = String.Empty;
        return tab;
    }

    public SlickTab Add(string caption)
    {
        SlickTab tab = CreateBlankTab();
        tab.Caption = caption;
        tab.Data = string.Empty;
        return tab;
    }

    public SlickTab Add(string caption, string initial_data)
    {
        SlickTab tab = CreateBlankTab();
        tab.Caption = caption;
        tab.Data = initial_data;
        return tab;
    }

    public void Remove(int index)
    {
        if (SelectedIndex == index)
        {
            SlickTab LastSelectedItem = SelectedItem;

            SelectedItem = Tabs[--SelectedIndex];
            Update();

            if (OnSelect != null)
            {
                OnSelect.DynamicInvoke(SelectedItem, LastSelectedItem);
            }
        }

        Parent.Controls.Remove(Tabs[index].btn_x);
        Parent.Controls.Remove(Tabs[index].lbl_caption);
        Parent.Controls.Remove(Tabs[index].tab_bg);

        Tabs[index].Active = false;
        Tabs.RemoveAt(index);
    }

    public void Remove(SlickTab tab)
    {
        if (SelectedItem == tab)
        {
            SlickTab LastSelectedItem = SelectedItem;

            SelectedItem = Tabs[--SelectedIndex];
            Update();

            if (OnSelect != null)
            {
                OnSelect.DynamicInvoke(SelectedItem, LastSelectedItem);
            }
        }

        Parent.Controls.Remove(tab.btn_x);
        Parent.Controls.Remove(tab.lbl_caption);
        Parent.Controls.Remove(tab.tab_bg);

        tab.Active = false;
        Tabs.Remove(tab);
    }

    public void Swap(int a_index, int b_index)
    {
        SlickTab tab = Tabs[a_index];
        Tabs.RemoveAt(a_index);
        Tabs.Insert(b_index, tab);
    }


    public void Update()
    {
        for (int i = 0; i < Tabs.Count; i++)
        {
            Tabs[i].Selected = false;

            int xpos = X;

            if (i > 0)
            {
                xpos = Tabs[i - 1].tab_bg.Right + 4; // i * Tabs[i].tab_bg.Width;
            }

            if (Tabs[i] == SelectedItem)
            {
                Tabs[i].Selected = true;
            }

            Tabs[i].Update(xpos, Y);
        }

        if (Tabs.Count > 0)
        {
            btn_open.Location = new Point(Tabs[Tabs.Count - 1].tab_bg.Right, Y - 1);
        } else
        {
            btn_open.Location = new Point(X, Y - 1);
        }
    }
}
