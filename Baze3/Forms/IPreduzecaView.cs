using Baze3.Domain;
using System;
using System.Collections.Generic;

namespace App.Views
{
    public interface IPreduzecaView
    {
        event EventHandler LoadRequested;
        event EventHandler<string> SearchByNazivRequested;
        event EventHandler<Preduzece> AddRequested;
        event EventHandler<Preduzece> EditRequested;
        event EventHandler<string> DeleteRequested;
        void Render(IEnumerable<Preduzece> data);
        void ClearEditor();
        void ShowError(string message);
    }
}
