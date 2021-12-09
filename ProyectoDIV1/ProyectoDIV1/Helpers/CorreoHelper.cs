using System;
using System.Diagnostics;
using System.IO;
using System.Net.Mail;
using System.Reflection;

namespace ProyectoDIV1.Helpers
{
    public class CorreoHelper
    {
        public void enviarCorreo(String correoDestino, String Usuario, String asunto, String mensaje)
        {
            try
            {
                string path = "ArchivosLocales.FormatoCorreo.html";
                var assembly = typeof(MasterCandidatoPage).GetTypeInfo().Assembly;

                var rutaCompleta = Path.Combine($"{assembly.GetName().Name}.{path}");
                Stream stream = assembly.GetManifestResourceStream(rutaCompleta);
                var Emailtemplate = new StreamReader(stream);
                var strBody = string.Format(Emailtemplate.ReadToEnd(), Usuario);
                Emailtemplate.Close();
                Emailtemplate.Dispose();
                Emailtemplate = null;
                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");
                //Especificamos el correo desde el que se enviará el Email y el nombre de la persona que lo envía
                mail.From = new MailAddress("tudec2020@gmail.com", "ProyectoDIV");
                SmtpServer.Host = "smtp.gmail.com";
                //Aquí ponemos el asunto del correo
                mail.Subject = asunto;
                //Aquí ponemos el mensaje que incluirá el correo

                strBody = strBody.Replace("TextoBody", $"{mensaje}");
                strBody = strBody.Replace("userId", $"{Usuario}");
                mail.Body = strBody;
                //mail.Body = "Por favor ingrese al siguiente link para recuperar su contraseña";
                //Especificamos a quien enviaremos el Email, no es necesario que sea Gmail, puede ser cualquier otro proveedor
                mail.To.Add(correoDestino);
                //Si queremos enviar archivos adjuntos tenemos que especificar la ruta en donde se encuentran
                //mail.Attachments.Add(new Attachment(@"C:\Documentos\carta.docx"));
                mail.IsBodyHtml = true;
                mail.Priority = MailPriority.Normal;
                //Configuracion del SMTP
                SmtpServer.EnableSsl = true;
                SmtpServer.Port = 587; //Puerto que utiliza Gmail para sus servicios
                                       //Especificamos las credenciales con las que enviaremos el mail
                SmtpServer.DeliveryMethod = SmtpDeliveryMethod.Network;
                SmtpServer.UseDefaultCredentials = false;
                SmtpServer.Credentials = new System.Net.NetworkCredential("tudec2020@gmail.com", "programadoresudec2020");
               
                SmtpServer.Send(mail);
                mail.Dispose();
                Toasts.Warning("Se finalizo el contrato y se envio notificación y email al usuario para la calificación.", 4000);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                Toasts.Warning("Se finalizo el contrato y se envio notificación y email al usuario para la calificación.", 4000);
                Toasts.Error("no se pudo enviar el correo.", 2000);
            }
        }
    }
}
