﻿using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Xml.Serialization;
using taskt.UI.CustomControls;
using taskt.UI.Forms;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Text File Commands")]
    [Attributes.ClassAttributes.Description("This command reads text data into a variable")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to read data from text files.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements '' to achieve automation.")]
    public class ReadTextFileCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please indicate the path to the file")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowFileSelectionHelper)]
        [Attributes.PropertyAttributes.InputSpecification("Enter or Select the path to the text file.")]
        [Attributes.PropertyAttributes.SampleUsage("C:\\temp\\myfile.txt or [vTextFilePath]")]
        [Attributes.PropertyAttributes.Remarks("")]
        public string v_FilePath { get; set; }

        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please select the read type")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Content")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Line Count")]
        [Attributes.PropertyAttributes.InputSpecification("Select the appropriate window state required")]
        [Attributes.PropertyAttributes.SampleUsage("Choose from **Minimize**, **Maximize** and **Restore**")]
        [Attributes.PropertyAttributes.Remarks("")]
        public string v_ReadOption { get; set; }

        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please define where the text should be stored")]
        [Attributes.PropertyAttributes.InputSpecification("Select or provide a variable from the variable list")]
        [Attributes.PropertyAttributes.SampleUsage("**vSomeVariable**")]
        [Attributes.PropertyAttributes.Remarks("If you have enabled the setting **Create Missing Variables at Runtime** then you are not required to pre-define your variables, however, it is highly recommended.")]
        public string v_userVariableName { get; set; }


        public ReadTextFileCommand()
        {
            this.CommandName = "ReadTextFileCommand";
            this.SelectionName = "Read Text File";
            this.CommandEnabled = true;
            this.CustomRendering = true;
            this.v_ReadOption = "Content";
        }

        public override void RunCommand(object sender)
        {
            //convert variables
            var filePath = v_FilePath.ConvertToUserVariable(sender);

            var readPreference = v_ReadOption.ConvertToUserVariable(sender).ToUpperInvariant();

            string result;
            if (readPreference == "LINE COUNT")
            {
                //read text from file
                result = System.IO.File.ReadAllLines(filePath).Length.ToString();
            }
            else
            {
                //read text from file
                result = System.IO.File.ReadAllText(filePath);
            }


            //assign text to user variable
            result.StoreInUserVariable(sender, v_userVariableName);
        }
        public override List<Control> Render(frmCommandEditor editor)
        {
            base.Render(editor);

            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_FilePath", this, editor));

            RenderedControls.AddRange(CommandControls.CreateDefaultDropdownGroupFor("v_ReadOption", this, editor));

            RenderedControls.Add(CommandControls.CreateDefaultLabelFor("v_userVariableName", this));
            var VariableNameControl = CommandControls.CreateStandardComboboxFor("v_userVariableName", this).AddVariableNames(editor);
            RenderedControls.AddRange(CommandControls.CreateUIHelpersFor("v_userVariableName", this, new Control[] { VariableNameControl }, editor));
            RenderedControls.Add(VariableNameControl);

            return RenderedControls;
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + " [Read " + v_ReadOption + " from '" + v_FilePath + "']";
        }
    }
}