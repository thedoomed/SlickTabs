using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

public class SlickTab
{
    public System.Windows.Forms.Label btn_x;
    public System.Windows.Forms.Label lbl_caption;
    public System.Windows.Forms.PictureBox tab_bg;
    public Color BackColor;
    public Color SelectedColor;

    public string Caption;
    public bool TextScale;
    public bool Selected;
    public bool Active;

    public object Data;

    // Create an empty container where we initialise the elements
    public SlickTab()
    {
        Active = true;

        // SlickTabControl will adjust properties
        btn_x = new System.Windows.Forms.Label();
        lbl_caption = new System.Windows.Forms.Label();
        tab_bg = new System.Windows.Forms.PictureBox();

        Caption = "Untitled";
        TextScale = false;
        Selected = false;
    }

    public void Update(int referenceX, int referenceY)
    {
        lbl_caption.Text = Caption;

        if (TextScale)
        {
            tab_bg.Size = new System.Drawing.Size(106 + Caption.Length, 21);
        }

        if (BackColor != null && SelectedColor != null)
        {
            if (Selected)
            {
                tab_bg.BackColor = BackColor;
            } else 
            {
                tab_bg.BackColor = SelectedColor;
            }
        }

        lbl_caption.BackColor = tab_bg.BackColor;
        btn_x.BackColor = tab_bg.BackColor;
        tab_bg.Location = new System.Drawing.Point(referenceX, referenceY);
        lbl_caption.Location = new System.Drawing.Point(referenceX + 8, referenceY + 3);
        btn_x.Location = new System.Drawing.Point(referenceX + (tab_bg.Width - 20), referenceY + 2);
    }
}
