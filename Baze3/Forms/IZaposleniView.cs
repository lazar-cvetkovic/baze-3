using Baze3.Domain;
using System;
using System.Collections.Generic;

namespace App.Views
{
    public interface IZaposleniView
    {
        event EventHandler LoadRequested;
        event EventHandler<string> SearchByImeRequested;
        event EventHandler<string> SearchByPrezimeRequested;
        event EventHandler<Zaposleni> AddRequested;
        event EventHandler<Zaposleni> EditRequested;
        event EventHandler<string> DeleteRequested;
        event EventHandler PickAddressRequested;

        void Render(IEnumerable<Zaposleni> data);
        void ClearEditor();
        void ShowError(string message);
        void SetAddress(int rbAdrese);
    }
}
