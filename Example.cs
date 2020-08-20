namespace Example
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        
        public SlickTabControl scriptTabs;
        
        private void Form1_Load(object sender, EventArgs e)
        {
            scriptTabs = new SlickTabControl(this); // set its parent to 'this' (our form)
            scriptTabs.SetPosition(0, 40); // Position for it on the form
            
            // Add(Title, Content(string))
            scriptTabs.Add("Untitled", "print(\"Hello, world\")");
            scriptTabs.Add("Test", "printidentity()");
            
            // Initially update and render it
            scriptTabs.Update();
            
            scriptTabs.OnSelect += (o, last) =>
            {
                SlickTab tab = (SlickTab)o;
                MessageBox.Show((string)tab.Data);
            };
            
        }
    }
}
