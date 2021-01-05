using CodeCreate.PostgreSql;
using CodeCreate.Service;
using EntityCreate.Domain;
using EntityCreate.Domain.EntityCreate;
using EntityCreate.Domain.Models;
using EntityCreate.PostgreSql;
using ICSharpCode.TextEditor.Document;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CodeCreate
{
    public partial class Form1 : Form
    {
        private IEntityProvrder  _entityProvrder;
        public Form1()
        { 
            _entityProvrder = new PostgreSqlProvrder("Server=sg-connect-staging.chdywxilu9oq.ap-southeast-1.rds.amazonaws.com;Database=wechatService;Port=5432;User Id=postgres;Password=38tSwG4af7TJqqs9HyPbA2C;Integrated Security=true;Pooling=true;");
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            var resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            var defaultTextEditorProperties1 = new DefaultTextEditorProperties
            {
                AllowCaretBeyondEOL = false,
                AutoInsertCurlyBracket = true,
                BracketMatchingStyle = BracketMatchingStyle.After,
                ConvertTabsToSpaces = false,
                DocumentSelectionMode = DocumentSelectionMode.Normal,
                EnableFolding = true,
                Encoding = ((System.Text.Encoding)(resources.GetObject("defaultTextEditorProperties1.Encoding"))),
                Font = new System.Drawing.Font("Courier New", 10F),
                HideMouseCursor = false,
                IndentStyle = IndentStyle.Smart,
                IsIconBarVisible = true,
                LineTerminator = "\r\n",
                LineViewerStyle = LineViewerStyle.None,
                MouseWheelScrollDown = true,
                MouseWheelTextZoom = true,
                ShowEOLMarker = true,
                ShowHorizontalRuler = false,
                ShowInvalidLines = true,
                ShowLineNumbers = true,
                ShowMatchingBracket = true,
                ShowSpaces = true,
                ShowTabs = true,
                ShowVerticalRuler = true,
                TabIndent = 4,
                VerticalRulerRow = 80,

            };
            this.textEditorControl1.TextEditorProperties = defaultTextEditorProperties1;
            this.textEditorControl1.Document.HighlightingStrategy = HighlightingStrategyFactory.CreateHighlightingStrategy("C#");
            this.textEditorControl1.Encoding = Encoding.Default;
            //this.textEditorControl1.Text = $@"
            //using ICSharpCode.TextEditor.Document;
            //using System;
            //using System.Collections.Generic;
            //using System.ComponentModel;
            //using System.Data;
            //using System.Drawing;
            //using System.Linq;
            //using System.Text;
            //using System.Threading.Tasks;
            //using System.Windows.Forms; ";
             var tables = _entityProvrder.GetTabses().ToArray();
            this.listBox1.Items.AddRange(tables);
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            var s = sender as ListBox;
            if (s == null) return;
            
            var table =  s.SelectedItem.ToString();
            var columnList = _entityProvrder.GetColumnInfos(table);
             
            var  entityCreate = new AbpEntityCreate();
            var str =  entityCreate.CreateEntity(table, columnList,new EntityConfig() { Namespace = txt_NameSpace.Text });
            textEditorControl1.Text = str;

        }

        private void textEditorControl1_Load(object sender, EventArgs e)
        {

        }
    }
}
