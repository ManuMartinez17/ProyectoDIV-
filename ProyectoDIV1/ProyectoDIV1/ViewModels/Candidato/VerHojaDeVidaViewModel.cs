using Newtonsoft.Json;
using ProyectoDIV1.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ProyectoDIV1.ViewModels.Candidato
{
    public class VerHojaDeVidaViewModel : BaseViewModel
    {
        private Stream m_pdfDocumentStream;
        public VerHojaDeVidaViewModel()
        {
            m_pdfDocumentStream = CargarDocumento();
        }

        private Stream CargarDocumento()
        {
            string url = JsonConvert.DeserializeObject<string>(Settings.Url);
            if (string.IsNullOrEmpty(url))
            {
                return null;
            }
            var archivo = ArchivoAStream.GetStreamFromUrl(url);
            return archivo;
        }

        public Stream PdfDocumentStream
        {
            get { return m_pdfDocumentStream; }
            set
            {
                SetProperty(ref m_pdfDocumentStream, value);
            }
        }
    }
}
