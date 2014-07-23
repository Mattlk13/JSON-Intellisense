﻿using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using EnvDTE;
using EnvDTE80;
using Microsoft.JSON.Core.Parser;
using Microsoft.JSON.Editor.Completion;
using Microsoft.JSON.Editor.Completion.Def;
using Microsoft.VisualStudio.Shell;
using Microsoft.Web.Editor.Intellisense;

namespace JSON_Intellisense._Shared.Completion
{
    public abstract class CompletionProviderBase : IJSONCompletionListProvider
    {
        [Import]
        private SVsServiceProvider serviceProvider { get; set; }

        protected static DTE2 _dte {get; private set;}

        public abstract JSONCompletionContextType ContextType { get; }

        public abstract string SupportedFileName { get; }

        public IEnumerable<CompletionEntry> GetListEntries(JSONCompletionContext context)
        {
            if (_dte == null)
                _dte = serviceProvider.GetService(typeof(DTE)) as DTE2;

            if (!Helper.IsSupportedFile(_dte, SupportedFileName))
                return new List<CompletionEntry>();

            return GetEntries(context);
        }

        protected abstract IEnumerable<CompletionEntry> GetEntries(JSONCompletionContext context);

        protected JSONMember GetDependency(JSONCompletionContext context)
        {
            JSONMember dependency = context.ContextItem.FindType<JSONMember>();
            JSONMember parent = dependency.Parent.FindType<JSONMember>();

            if (parent == null || !parent.Name.Text.Trim('"').EndsWith("dependencies", StringComparison.OrdinalIgnoreCase))
                return null;

            return dependency;
        }
    }
}