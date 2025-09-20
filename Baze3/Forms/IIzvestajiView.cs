using Baze3.Domain;
using System;
using System.Collections.Generic;

namespace App.Views
{
    public interface IIzvestajiView
    {
        event EventHandler LoadRequested;
        event EventHandler<string> SearchRequested;
        event EventHandler<IzvestajZaposlenog> AddRequested;
        event EventHandler<IzvestajZaposlenog> EditRequested;
        event EventHandler<IzvestajZaposlenog> DownloadPdfRequested;
        void Render(IEnumerable<IzvestajZaposlenog> data);
        void ClearEditor();
        void ShowError(string message);
    }
}
