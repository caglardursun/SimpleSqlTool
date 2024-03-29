﻿// ------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version: 16.0.0.0
//  
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
// ------------------------------------------------------------------------------
namespace SqlGen.Templates
{
    using System.Linq;
    using System.Text;
    using System.Collections;
    using System.Collections.Generic;
    using SqlGen;
    using System;
    
    /// <summary>
    /// Class to produce the template output
    /// </summary>
    
    #line 1 "D:\Projects\SimpleSqlTool\SqlGen\Templates\UpdateCommandHandlerTemplates.tt"
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.TextTemplating", "16.0.0.0")]
    public partial class UpdateCommandHandlerTemplates : UpdateCommandHandlerTemplatesBase
    {
#line hidden
        /// <summary>
        /// Create the template output
        /// </summary>
        public virtual string TransformText()
        {
            this.Write("using AutoMapper;\r\nusing MediatR;\r\nusing System;\r\nusing System.Threading;\r\nusing " +
                    "System.Threading.Tasks;\r\nusing Sbu.Ubys.Core.Aspects.Autofac.Validation;\r\nusing " +
                    "Sbu.Ubys.Core.Utilities.Results;\r\nusing Sbu.Ubys.Core.Localization.Constants;\r\nu" +
                    "sing ");
            
            #line 21 "D:\Projects\SimpleSqlTool\SqlGen\Templates\UpdateCommandHandlerTemplates.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(_namespace));
            
            #line default
            #line hidden
            this.Write(".Business.Handlers.");
            
            #line 21 "D:\Projects\SimpleSqlTool\SqlGen\Templates\UpdateCommandHandlerTemplates.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(tableName));
            
            #line default
            #line hidden
            this.Write("Handlers.ValidationRules;\r\nusing ");
            
            #line 22 "D:\Projects\SimpleSqlTool\SqlGen\Templates\UpdateCommandHandlerTemplates.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(_namespace));
            
            #line default
            #line hidden
            this.Write(".DataAccess.Abstract;\r\nusing ");
            
            #line 23 "D:\Projects\SimpleSqlTool\SqlGen\Templates\UpdateCommandHandlerTemplates.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(_namespace));
            
            #line default
            #line hidden
            this.Write(".Entities.Concrete;\r\nusing ");
            
            #line 24 "D:\Projects\SimpleSqlTool\SqlGen\Templates\UpdateCommandHandlerTemplates.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(_namespace));
            
            #line default
            #line hidden
            this.Write(".Entities.RequestDtos.");
            
            #line 24 "D:\Projects\SimpleSqlTool\SqlGen\Templates\UpdateCommandHandlerTemplates.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(tableName));
            
            #line default
            #line hidden
            this.Write(";\r\nusing ");
            
            #line 25 "D:\Projects\SimpleSqlTool\SqlGen\Templates\UpdateCommandHandlerTemplates.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(_namespace));
            
            #line default
            #line hidden
            this.Write(".Business.BusinessAspects;\r\nusing ");
            
            #line 26 "D:\Projects\SimpleSqlTool\SqlGen\Templates\UpdateCommandHandlerTemplates.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(_namespace));
            
            #line default
            #line hidden
            this.Write(".Data.Entity;\r\n\r\nnamespace ");
            
            #line 28 "D:\Projects\SimpleSqlTool\SqlGen\Templates\UpdateCommandHandlerTemplates.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(_namespace));
            
            #line default
            #line hidden
            this.Write(".Business.Handlers.");
            
            #line 28 "D:\Projects\SimpleSqlTool\SqlGen\Templates\UpdateCommandHandlerTemplates.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(tableName));
            
            #line default
            #line hidden
            this.Write("Handlers.Commands\r\n{\r\n\t\r\n\tpublic class Update");
            
            #line 31 "D:\Projects\SimpleSqlTool\SqlGen\Templates\UpdateCommandHandlerTemplates.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(tableName));
            
            #line default
            #line hidden
            this.Write("Command : IRequest<IResult>\r\n\t{\r\n\t\t\tpublic Update");
            
            #line 33 "D:\Projects\SimpleSqlTool\SqlGen\Templates\UpdateCommandHandlerTemplates.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(tableName));
            
            #line default
            #line hidden
            this.Write("RequestDto model { get; set; }\t\r\n\t}\r\n\t\r\n\tpublic class Update");
            
            #line 36 "D:\Projects\SimpleSqlTool\SqlGen\Templates\UpdateCommandHandlerTemplates.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(tableName));
            
            #line default
            #line hidden
            this.Write("CommandHandler : IRequestHandler<Update");
            
            #line 36 "D:\Projects\SimpleSqlTool\SqlGen\Templates\UpdateCommandHandlerTemplates.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(tableName));
            
            #line default
            #line hidden
            this.Write("Command, IResult>{\r\n\t\r\n\t\tprivate readonly IMapper _mapper;\r\n\t\tprivate readonly I");
            
            #line 39 "D:\Projects\SimpleSqlTool\SqlGen\Templates\UpdateCommandHandlerTemplates.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(tableName));
            
            #line default
            #line hidden
            this.Write("Repository _");
            
            #line 39 "D:\Projects\SimpleSqlTool\SqlGen\Templates\UpdateCommandHandlerTemplates.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(tableNameToLower));
            
            #line default
            #line hidden
            this.Write("Repository;\r\n\t\tprivate readonly IMediator _mediator;\r\n\t\t\r\n\t\tpublic Update");
            
            #line 42 "D:\Projects\SimpleSqlTool\SqlGen\Templates\UpdateCommandHandlerTemplates.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(tableName));
            
            #line default
            #line hidden
            this.Write("CommandHandler(\r\n\t\t\tIMapper mapper,\r\n\t\t\tI");
            
            #line 44 "D:\Projects\SimpleSqlTool\SqlGen\Templates\UpdateCommandHandlerTemplates.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(tableName));
            
            #line default
            #line hidden
            this.Write("Repository repository,\r\n\t\t\tIMediator mediator\r\n\t\t){\r\n\t\t\t\t_mapper = mapper;\r\n\t\t\t\t_" +
                    "");
            
            #line 48 "D:\Projects\SimpleSqlTool\SqlGen\Templates\UpdateCommandHandlerTemplates.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(tableNameToLower));
            
            #line default
            #line hidden
            this.Write("Repository = repository;\r\n\t\t\t\t_mediator = mediator;\r\n\t\t}\r\n\t\t\r\n\t\t\r\n\t\t[ValidationAs" +
                    "pect(typeof(Update");
            
            #line 53 "D:\Projects\SimpleSqlTool\SqlGen\Templates\UpdateCommandHandlerTemplates.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(tableName));
            
            #line default
            #line hidden
            this.Write("RequestValidator), Priority = 1)]\r\n\t\t[SecuredOperation(Priority = 1)]\r\n\t\tpublic a" +
                    "sync Task<IResult> Handle(Update");
            
            #line 55 "D:\Projects\SimpleSqlTool\SqlGen\Templates\UpdateCommandHandlerTemplates.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(tableName));
            
            #line default
            #line hidden
            this.Write("Command request, CancellationToken cancellationToken)\r\n\t\t{\r\n\t\t\ttry{\r\n\t\t\t\t\r\n\t\t\r\n\t\t" +
                    "\t\tvar item = _mapper.Map<Update");
            
            #line 60 "D:\Projects\SimpleSqlTool\SqlGen\Templates\UpdateCommandHandlerTemplates.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(tableName));
            
            #line default
            #line hidden
            this.Write("RequestDto, ");
            
            #line 60 "D:\Projects\SimpleSqlTool\SqlGen\Templates\UpdateCommandHandlerTemplates.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(tableName));
            
            #line default
            #line hidden
            this.Write(">(request.model);\r\n\t\t\r\n\t\t\t\t_");
            
            #line 62 "D:\Projects\SimpleSqlTool\SqlGen\Templates\UpdateCommandHandlerTemplates.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(tableNameToLower));
            
            #line default
            #line hidden
            this.Write("Repository.Update(item);\r\n\t\t\r\n\t\t\t\tawait _");
            
            #line 64 "D:\Projects\SimpleSqlTool\SqlGen\Templates\UpdateCommandHandlerTemplates.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(tableNameToLower));
            
            #line default
            #line hidden
            this.Write("Repository.SaveChangesAsync();\r\n\t\t\t\treturn new SuccessResult(Resource.BasariliGun" +
                    "cellemeIslemi);\r\n\t\t\r\n\t\t\t}catch(Exception exc)\r\n\t\t\t{\r\n\t\t\t\treturn new ErrorResult(" +
                    "exc.InnerException.Message);\r\n\t\t\t}\r\n\t\t}\r\n\t\t\t\r\n\t}\r\n}");
            return this.GenerationEnvironment.ToString();
        }
        
        #line 1 "D:\Projects\SimpleSqlTool\SqlGen\Templates\UpdateCommandHandlerTemplates.tt"

private string @__namespaceField;

/// <summary>
/// Access the _namespace parameter of the template.
/// </summary>
private string _namespace
{
    get
    {
        return this.@__namespaceField;
    }
}

private string _tableNameField;

/// <summary>
/// Access the tableName parameter of the template.
/// </summary>
private string tableName
{
    get
    {
        return this._tableNameField;
    }
}

private string _tableNameToLowerField;

/// <summary>
/// Access the tableNameToLower parameter of the template.
/// </summary>
private string tableNameToLower
{
    get
    {
        return this._tableNameToLowerField;
    }
}

private global::System.Collections.Generic.IEnumerable<Column> _columnsField;

/// <summary>
/// Access the columns parameter of the template.
/// </summary>
private global::System.Collections.Generic.IEnumerable<Column> columns
{
    get
    {
        return this._columnsField;
    }
}

private string _tableNameToPascalField;

/// <summary>
/// Access the tableNameToPascal parameter of the template.
/// </summary>
private string tableNameToPascal
{
    get
    {
        return this._tableNameToPascalField;
    }
}


/// <summary>
/// Initialize the template
/// </summary>
public virtual void Initialize()
{
    if ((this.Errors.HasErrors == false))
    {
bool _namespaceValueAcquired = false;
if (this.Session.ContainsKey("_namespace"))
{
    this.@__namespaceField = ((string)(this.Session["_namespace"]));
    _namespaceValueAcquired = true;
}
if ((_namespaceValueAcquired == false))
{
    object data = global::System.Runtime.Remoting.Messaging.CallContext.LogicalGetData("_namespace");
    if ((data != null))
    {
        this.@__namespaceField = ((string)(data));
    }
}
bool tableNameValueAcquired = false;
if (this.Session.ContainsKey("tableName"))
{
    this._tableNameField = ((string)(this.Session["tableName"]));
    tableNameValueAcquired = true;
}
if ((tableNameValueAcquired == false))
{
    object data = global::System.Runtime.Remoting.Messaging.CallContext.LogicalGetData("tableName");
    if ((data != null))
    {
        this._tableNameField = ((string)(data));
    }
}
bool tableNameToLowerValueAcquired = false;
if (this.Session.ContainsKey("tableNameToLower"))
{
    this._tableNameToLowerField = ((string)(this.Session["tableNameToLower"]));
    tableNameToLowerValueAcquired = true;
}
if ((tableNameToLowerValueAcquired == false))
{
    object data = global::System.Runtime.Remoting.Messaging.CallContext.LogicalGetData("tableNameToLower");
    if ((data != null))
    {
        this._tableNameToLowerField = ((string)(data));
    }
}
bool columnsValueAcquired = false;
if (this.Session.ContainsKey("columns"))
{
    this._columnsField = ((global::System.Collections.Generic.IEnumerable<Column>)(this.Session["columns"]));
    columnsValueAcquired = true;
}
if ((columnsValueAcquired == false))
{
    object data = global::System.Runtime.Remoting.Messaging.CallContext.LogicalGetData("columns");
    if ((data != null))
    {
        this._columnsField = ((global::System.Collections.Generic.IEnumerable<Column>)(data));
    }
}
bool tableNameToPascalValueAcquired = false;
if (this.Session.ContainsKey("tableNameToPascal"))
{
    this._tableNameToPascalField = ((string)(this.Session["tableNameToPascal"]));
    tableNameToPascalValueAcquired = true;
}
if ((tableNameToPascalValueAcquired == false))
{
    object data = global::System.Runtime.Remoting.Messaging.CallContext.LogicalGetData("tableNameToPascal");
    if ((data != null))
    {
        this._tableNameToPascalField = ((string)(data));
    }
}


    }
}


        
        #line default
        #line hidden
    }
    
    #line default
    #line hidden
    #region Base class
    /// <summary>
    /// Base class for this transformation
    /// </summary>
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.TextTemplating", "16.0.0.0")]
    public class UpdateCommandHandlerTemplatesBase
    {
        #region Fields
        private global::System.Text.StringBuilder generationEnvironmentField;
        private global::System.CodeDom.Compiler.CompilerErrorCollection errorsField;
        private global::System.Collections.Generic.List<int> indentLengthsField;
        private string currentIndentField = "";
        private bool endsWithNewline;
        private global::System.Collections.Generic.IDictionary<string, object> sessionField;
        #endregion
        #region Properties
        /// <summary>
        /// The string builder that generation-time code is using to assemble generated output
        /// </summary>
        protected System.Text.StringBuilder GenerationEnvironment
        {
            get
            {
                if ((this.generationEnvironmentField == null))
                {
                    this.generationEnvironmentField = new global::System.Text.StringBuilder();
                }
                return this.generationEnvironmentField;
            }
            set
            {
                this.generationEnvironmentField = value;
            }
        }
        /// <summary>
        /// The error collection for the generation process
        /// </summary>
        public System.CodeDom.Compiler.CompilerErrorCollection Errors
        {
            get
            {
                if ((this.errorsField == null))
                {
                    this.errorsField = new global::System.CodeDom.Compiler.CompilerErrorCollection();
                }
                return this.errorsField;
            }
        }
        /// <summary>
        /// A list of the lengths of each indent that was added with PushIndent
        /// </summary>
        private System.Collections.Generic.List<int> indentLengths
        {
            get
            {
                if ((this.indentLengthsField == null))
                {
                    this.indentLengthsField = new global::System.Collections.Generic.List<int>();
                }
                return this.indentLengthsField;
            }
        }
        /// <summary>
        /// Gets the current indent we use when adding lines to the output
        /// </summary>
        public string CurrentIndent
        {
            get
            {
                return this.currentIndentField;
            }
        }
        /// <summary>
        /// Current transformation session
        /// </summary>
        public virtual global::System.Collections.Generic.IDictionary<string, object> Session
        {
            get
            {
                return this.sessionField;
            }
            set
            {
                this.sessionField = value;
            }
        }
        #endregion
        #region Transform-time helpers
        /// <summary>
        /// Write text directly into the generated output
        /// </summary>
        public void Write(string textToAppend)
        {
            if (string.IsNullOrEmpty(textToAppend))
            {
                return;
            }
            // If we're starting off, or if the previous text ended with a newline,
            // we have to append the current indent first.
            if (((this.GenerationEnvironment.Length == 0) 
                        || this.endsWithNewline))
            {
                this.GenerationEnvironment.Append(this.currentIndentField);
                this.endsWithNewline = false;
            }
            // Check if the current text ends with a newline
            if (textToAppend.EndsWith(global::System.Environment.NewLine, global::System.StringComparison.CurrentCulture))
            {
                this.endsWithNewline = true;
            }
            // This is an optimization. If the current indent is "", then we don't have to do any
            // of the more complex stuff further down.
            if ((this.currentIndentField.Length == 0))
            {
                this.GenerationEnvironment.Append(textToAppend);
                return;
            }
            // Everywhere there is a newline in the text, add an indent after it
            textToAppend = textToAppend.Replace(global::System.Environment.NewLine, (global::System.Environment.NewLine + this.currentIndentField));
            // If the text ends with a newline, then we should strip off the indent added at the very end
            // because the appropriate indent will be added when the next time Write() is called
            if (this.endsWithNewline)
            {
                this.GenerationEnvironment.Append(textToAppend, 0, (textToAppend.Length - this.currentIndentField.Length));
            }
            else
            {
                this.GenerationEnvironment.Append(textToAppend);
            }
        }
        /// <summary>
        /// Write text directly into the generated output
        /// </summary>
        public void WriteLine(string textToAppend)
        {
            this.Write(textToAppend);
            this.GenerationEnvironment.AppendLine();
            this.endsWithNewline = true;
        }
        /// <summary>
        /// Write formatted text directly into the generated output
        /// </summary>
        public void Write(string format, params object[] args)
        {
            this.Write(string.Format(global::System.Globalization.CultureInfo.CurrentCulture, format, args));
        }
        /// <summary>
        /// Write formatted text directly into the generated output
        /// </summary>
        public void WriteLine(string format, params object[] args)
        {
            this.WriteLine(string.Format(global::System.Globalization.CultureInfo.CurrentCulture, format, args));
        }
        /// <summary>
        /// Raise an error
        /// </summary>
        public void Error(string message)
        {
            System.CodeDom.Compiler.CompilerError error = new global::System.CodeDom.Compiler.CompilerError();
            error.ErrorText = message;
            this.Errors.Add(error);
        }
        /// <summary>
        /// Raise a warning
        /// </summary>
        public void Warning(string message)
        {
            System.CodeDom.Compiler.CompilerError error = new global::System.CodeDom.Compiler.CompilerError();
            error.ErrorText = message;
            error.IsWarning = true;
            this.Errors.Add(error);
        }
        /// <summary>
        /// Increase the indent
        /// </summary>
        public void PushIndent(string indent)
        {
            if ((indent == null))
            {
                throw new global::System.ArgumentNullException("indent");
            }
            this.currentIndentField = (this.currentIndentField + indent);
            this.indentLengths.Add(indent.Length);
        }
        /// <summary>
        /// Remove the last indent that was added with PushIndent
        /// </summary>
        public string PopIndent()
        {
            string returnValue = "";
            if ((this.indentLengths.Count > 0))
            {
                int indentLength = this.indentLengths[(this.indentLengths.Count - 1)];
                this.indentLengths.RemoveAt((this.indentLengths.Count - 1));
                if ((indentLength > 0))
                {
                    returnValue = this.currentIndentField.Substring((this.currentIndentField.Length - indentLength));
                    this.currentIndentField = this.currentIndentField.Remove((this.currentIndentField.Length - indentLength));
                }
            }
            return returnValue;
        }
        /// <summary>
        /// Remove any indentation
        /// </summary>
        public void ClearIndent()
        {
            this.indentLengths.Clear();
            this.currentIndentField = "";
        }
        #endregion
        #region ToString Helpers
        /// <summary>
        /// Utility class to produce culture-oriented representation of an object as a string.
        /// </summary>
        public class ToStringInstanceHelper
        {
            private System.IFormatProvider formatProviderField  = global::System.Globalization.CultureInfo.InvariantCulture;
            /// <summary>
            /// Gets or sets format provider to be used by ToStringWithCulture method.
            /// </summary>
            public System.IFormatProvider FormatProvider
            {
                get
                {
                    return this.formatProviderField ;
                }
                set
                {
                    if ((value != null))
                    {
                        this.formatProviderField  = value;
                    }
                }
            }
            /// <summary>
            /// This is called from the compile/run appdomain to convert objects within an expression block to a string
            /// </summary>
            public string ToStringWithCulture(object objectToConvert)
            {
                if ((objectToConvert == null))
                {
                    throw new global::System.ArgumentNullException("objectToConvert");
                }
                System.Type t = objectToConvert.GetType();
                System.Reflection.MethodInfo method = t.GetMethod("ToString", new System.Type[] {
                            typeof(System.IFormatProvider)});
                if ((method == null))
                {
                    return objectToConvert.ToString();
                }
                else
                {
                    return ((string)(method.Invoke(objectToConvert, new object[] {
                                this.formatProviderField })));
                }
            }
        }
        private ToStringInstanceHelper toStringHelperField = new ToStringInstanceHelper();
        /// <summary>
        /// Helper to produce culture-oriented representation of an object as a string
        /// </summary>
        public ToStringInstanceHelper ToStringHelper
        {
            get
            {
                return this.toStringHelperField;
            }
        }
        #endregion
    }
    #endregion
}
