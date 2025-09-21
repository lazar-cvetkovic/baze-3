using Baze3.Domain;
using System;
using System.Collections.Generic;

namespace App.Views
{
    public interface IUgovoriView
    {
        event EventHandler LoadRequested;
        event EventHandler<string> SearchRequested;
        event EventHandler<UgovorORadu> AddRequested;
        event EventHandler<UgovorORadu> EditRequested;
        event EventHandler<UgovorORadu> DownloadPdfRequested;
        event EventHandler<UgovorORadu> OpenRequested;
        event EventHandler<UgovorORadu> CloseRequested;

        void Render(IEnumerable<UgovorORadu> data);
        void ClearEditor();
        void ShowError(string message);
    }
}
