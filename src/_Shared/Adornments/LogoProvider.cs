﻿using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Utilities;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;

namespace JSON_Intellisense
{
    [Export(typeof(IWpfTextViewCreationListener))]
    [ContentType("json")]
    [ContentType("javascript")]
    [TextViewRole(PredefinedTextViewRoles.Document)]
    class LogoProvider : IWpfTextViewCreationListener
    {
        public const string LayerName = "JSON Logo";

        [Import]
        public ITextDocumentFactoryService TextDocumentFactoryService { get; set; }

        [Export(typeof(AdornmentLayerDefinition))]
        [Name(LayerName)]
        [Order(After = PredefinedAdornmentLayers.Caret)]
        public AdornmentLayerDefinition editorAdornmentLayer = null;

        private Dictionary<string, string> _map = new Dictionary<string, string>()
        {
            { "bower.json", "bower.png"},
            { "package.json", "npm.png"},
            { "gruntfile.js", "grunt.png"},
            { "gulpfile.js", "gulp.png"},
        };


        public void TextViewCreated(IWpfTextView textView)
        {
            ITextDocument document;
            if (TextDocumentFactoryService.TryGetTextDocument(textView.TextDataModel.DocumentBuffer, out document))
            {
                string fileName = Path.GetFileName(document.FilePath).ToLowerInvariant();

                if (!string.IsNullOrEmpty(fileName) && _map.ContainsKey(fileName))
                {
                    LogoAdornment highlighter = new LogoAdornment(textView, _map[fileName]);
                }
            }
        }
    }
}
