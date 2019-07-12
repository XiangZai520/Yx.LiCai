using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace YxLiCai.FinaAdmin.Models
{
    public class EditTemplateViewModel
    {
    }
    public class KendoTextBoxViewModel
    {
        /// <summary>
        /// For label.
        /// </summary>
        public object Content { get; set; }

        /// <summary>
        /// For input.
        /// </summary>
        public string Id { get; set; }

        public object Value { get; set; }

        public string Readonly { get; set; }
    }
    public class KendoDropdownlistViewModel : KendoTextBoxViewModel
    {
        public string SourceUrl { get; set; }

        public string Index { get; set; }

        public string[] DataSourceParameter { get; set; }
    }
    public class KendoTreeDropViewModel : KendoTextBoxViewModel
    {
        public object SpanContent { get; set; }

        public object InputCurrent { get; set; }
    }
    public class KendoTextAreaViewModel : KendoTextBoxViewModel
    { }
    public class KendoTreeViewViewModel : KendoTextBoxViewModel
    { }
    public class KendoNumericTextBoxViewModel : KendoTextBoxViewModel
    {
        public string Format { get; set; }
        public string Decimals { get; set; }
        public string Min { get; set; }
        public string Max { get; set; }
    }
    public class KendoDateTimeViewModel : KendoTextBoxViewModel
    {
        public string Start { get; set; }

        public string Depth { get; set; }

        public string Format { get; set; }
        public DateTime? Min { get; set; }
        public DateTime? Max { get; set; } 
    }
    public class UEditorViewModel : KendoTextBoxViewModel
    {

    }
    public class RadioViewModel : KendoTextBoxViewModel
    {

    }
}