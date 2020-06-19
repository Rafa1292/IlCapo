using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Web;

namespace IlCapo.Models
{
    public class ticket
    {
        StringBuilder linea = new StringBuilder();
        int maxCar = 42;
        int cortar;

        public string LineasGuion()
        {
            string lineasGuion = "";
            for (int i = 0; i < maxCar; i++)
            {
                lineasGuion += "-";
            }
            return linea.AppendLine(lineasGuion).ToString();
        }

        public string LineasAsterisco()
        {
            string lineasAsterisco = "";
            for (int i = 0; i < maxCar; i++)
            {
                lineasAsterisco += "*";
            }
            return linea.AppendLine(lineasAsterisco).ToString();
        }

        public string LineasIgual()
        {
            string lineasIgual = "";
            for (int i = 0; i < maxCar; i++)
            {
                lineasIgual += "=";
            }
            return linea.AppendLine(lineasIgual).ToString();
        }

        public void Encabezado()
        {
            linea.AppendLine(" ARTICULO             |CANT|PRECIO|TOTAL");
        }

        public void TextoIzquierda(string texto)
        {
            if (texto.Length > maxCar)
            {
                int caracterActual = 0;
                for (int longitudTexto = texto.Length; longitudTexto > maxCar; longitudTexto -= maxCar)
                {
                    linea.AppendLine(texto.Substring(caracterActual, maxCar));
                    caracterActual += maxCar;
                }

                linea.AppendLine(texto.Substring(caracterActual, texto.Length - caracterActual));
            }
            else
            {
                linea.AppendLine(" " + texto);
            }
        }

        public void TextoDerecha(string texto)
        {
            if (texto.Length > maxCar)
            {
                int caracterActual = 0;
                for (int longitudTexto = texto.Length; longitudTexto > maxCar; longitudTexto -= maxCar)
                {
                    linea.AppendLine(texto.Substring(caracterActual, maxCar));
                    caracterActual += maxCar;
                }
                string espacios = "";
                for (int i = 0; i < (maxCar - texto.Substring(caracterActual, texto.Length - caracterActual).Length); i++)
                {
                    espacios += " ";
                }

                linea.AppendLine(espacios + texto.Substring(caracterActual, texto.Length - caracterActual));
            }
            else
            {
                string espacios = "";
                for (int i = 0; i < (maxCar - texto.Length); i++)
                {
                    espacios += " ";
                }

                linea.AppendLine(espacios + texto);
            }
        }

        public void TextoCentro(string texto)
        {
            if (texto.Length > maxCar)
            {
                int caracterActual = 0;
                for (int longitudTexto = texto.Length; longitudTexto > maxCar; longitudTexto -= maxCar)
                {
                    linea.AppendLine(texto.Substring(caracterActual, maxCar));
                    caracterActual += maxCar;
                }

                string espacios = "";

                int centrar = (maxCar - texto.Substring(caracterActual, texto.Length - caracterActual).Length) / 2;
                for (int i = 0; i < centrar; i++)
                {
                    espacios += " ";
                }

                linea.AppendLine(espacios + texto.Substring(caracterActual, texto.Length - caracterActual));
            }
            else
            {
                string espacios = "";

                int centrar = (maxCar - texto.Length) / 2;
                for (int i = 0; i < centrar; i++)
                {
                    espacios += " ";
                }

                linea.AppendLine(espacios + texto);
            }
        }

        public void TextoExtremos(string textoIzquierda, string textoDerecha)
        {
            string textoIzq, textoDer, textoCompleto = "", espacios = "";

            if (textoIzquierda.Length > 18)
            {
                cortar = textoIzquierda.Length - 18;
                textoIzq = textoIzquierda.Remove(18, cortar);
            }
            else
            {
                textoIzq = textoIzquierda;
            }

            if (textoDerecha.Length > 20)
            {
                cortar = textoDerecha.Length - 20;
                textoDer = textoDerecha.Remove(20, cortar);
            }
            else
            {
                textoDer = textoDerecha;
            }

            int nroEspacios = maxCar - (textoIzq.Length + textoDer.Length);

            for (int i = 0; i < nroEspacios; i++)
            {
                espacios += " ";
            }
            textoCompleto += espacios + textoDerecha;
            linea.AppendLine(textoCompleto);
        }

        public void AgregarTotales(string texto, decimal total)
        {
            string resumen, valor, textoCompleto, espacios = "";

            if (texto.Length > 30)
            {
                cortar = texto.Length - 30;
                resumen = texto.Remove(30, cortar);
            }
            else
            {
                resumen = texto;
            }

            textoCompleto = resumen;

            var initialPrice = total.ToString();
            var priceSize = initialPrice.Length - 3;
            for (int x = priceSize; x > 0; x -= 3)
            {
                initialPrice = initialPrice.Insert(x, ",");
            }
            valor = initialPrice;
            int nroEspacios = maxCar - (resumen.Length + valor.Length + 4);

            for (int i = 0; i < nroEspacios; i++)
            {
                espacios += " ";
            }

            textoCompleto += espacios + "CRC " + valor;
            linea.AppendLine(textoCompleto);
        }

        public void AgregarArticuloComanda(Product product, int cant, string descripcion, List<ItemExtra> extras, List<ItemSide> acompañamientos, int itemId)
        {
            linea.AppendLine($"  Linea #{itemId} ");

            string elemento = "  ";
            bool bandera = false;

            elemento += cant.ToString() + ": ";
            elemento += $"{product.ProductSubCategory.Name} {product.Name}";

            linea.AppendLine(elemento);



            if (acompañamientos.Count > 0)
            {
                linea.AppendLine("            Acompanamientos");

                foreach (var side in acompañamientos)
                {
                    linea.AppendLine($"          -{side.Sides.Name} (Pr.{side.ProductQuantity})");
                }
            }

            if (extras.Count > 0)
            {
                linea.AppendLine("                 Extras");

                foreach (var extra in extras)
                {
                    linea.AppendLine($"          {extra.Quantity}: {extra.Extra.Name} (Pr.{extra.ProductQuantity})");
                }
            }
            linea.AppendLine("");

            if (descripcion != "")
            {
                linea.AppendLine(" Detalles: " +descripcion);

            }

        }
        public void AgregarArticulo(string articulo, int cant, int precio, int total)
        {
            if (cant.ToString().Length <= 5 && precio.ToString().Length <= 7 && total.ToString().Length <= 8)
            {
                string elemento = "", espacios = "";
                bool bandera = false;
                int nroEspacios = 0;

                if (articulo.Length > 20)
                {
                    nroEspacios = (5 - cant.ToString().Length);
                    espacios = "";
                    for (int i = 0; i < nroEspacios; i++)
                    {
                        espacios += " ";
                    }
                    elemento += espacios + cant.ToString();

                    nroEspacios = (7 - precio.ToString().Length);
                    espacios = "";

                    for (int i = 0; i < nroEspacios; i++)
                    {
                        espacios += " ";
                    }
                    elemento += espacios + precio.ToString();

                    nroEspacios = (8 - total.ToString().Length);
                    espacios = "";

                    for (int i = 0; i < nroEspacios; i++)
                    {
                        espacios += " ";
                    }
                    elemento += espacios + total.ToString();

                    int caracterActual = 0;

                    for (int longitudTexto = articulo.Length; longitudTexto > 20; longitudTexto -= 20)
                    {
                        if (!bandera)
                        {
                            linea.AppendLine(articulo.Substring(caracterActual, 20) + elemento);
                            bandera = true;
                        }
                        else
                        {
                            linea.AppendLine(articulo.Substring(caracterActual, 20));
                            caracterActual += 20;
                        }
                    }

                    linea.AppendLine(articulo.Substring(caracterActual, articulo.Length - caracterActual));
                }
                else
                {
                    for (int i = 0; i < (20 - articulo.Length); i++)
                    {
                        espacios += " ";
                    }
                    elemento = articulo + espacios;

                    nroEspacios = (5 - cant.ToString().Length);
                    espacios = "";

                    for (int i = 0; i < nroEspacios; i++)
                    {
                        espacios += " ";
                    }
                    elemento += espacios + cant.ToString();

                    nroEspacios = (7 - precio.ToString().Length);
                    espacios = "";

                    for (int i = 0; i < nroEspacios; i++)
                    {
                        espacios += " ";
                    }
                    elemento += espacios + precio.ToString();

                    nroEspacios = (8 - total.ToString().Length);
                    espacios = "";

                    for (int i = 0; i < nroEspacios; i++)
                    {
                        espacios += " ";
                    }
                    elemento += espacios + total.ToString();

                    linea.AppendLine(elemento);
                }
            }
            else
            {
                linea.AppendLine("Los valores ingresados para esta fila");
                linea.AppendLine("superan las columnas soportadas.");
                throw new Exception("Valores demasiado  grandes");

            }
        }

        public void CortarTicket()
        {
            linea.AppendLine("\x1b" + "m");

        }

        public void ImprimirTicket(string impresora)
        {
            RawPrinterHelper.SendStringToPrinter(impresora, linea.ToString());
            linea.Clear();
        }
    }

    public class RawPrinterHelper
    {
        // Structure and API declarions:
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        public class DOCINFOA
        {
            [MarshalAs(UnmanagedType.LPStr)]
            public string pDocName;
            [MarshalAs(UnmanagedType.LPStr)]
            public string pOutputFile;
            [MarshalAs(UnmanagedType.LPStr)]
            public string pDataType;
        }
        [DllImport("winspool.Drv", EntryPoint = "OpenPrinterA", SetLastError = true, CharSet = CharSet.Ansi, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern bool OpenPrinter([MarshalAs(UnmanagedType.LPStr)] string szPrinter, out IntPtr hPrinter, IntPtr pd);

        [DllImport("winspool.Drv", EntryPoint = "ClosePrinter", SetLastError = true, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern bool ClosePrinter(IntPtr hPrinter);

        [DllImport("winspool.Drv", EntryPoint = "StartDocPrinterA", SetLastError = true, CharSet = CharSet.Ansi, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern bool StartDocPrinter(IntPtr hPrinter, Int32 level, [In, MarshalAs(UnmanagedType.LPStruct)] DOCINFOA di);

        [DllImport("winspool.Drv", EntryPoint = "EndDocPrinter", SetLastError = true, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern bool EndDocPrinter(IntPtr hPrinter);

        [DllImport("winspool.Drv", EntryPoint = "StartPagePrinter", SetLastError = true, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern bool StartPagePrinter(IntPtr hPrinter);

        [DllImport("winspool.Drv", EntryPoint = "EndPagePrinter", SetLastError = true, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern bool EndPagePrinter(IntPtr hPrinter);

        [DllImport("winspool.Drv", EntryPoint = "WritePrinter", SetLastError = true, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern bool WritePrinter(IntPtr hPrinter, IntPtr pBytes, Int32 dwCount, out Int32 dwWritten);

        // SendBytesToPrinter()
        // When the function is given a printer name and an unmanaged array
        // of bytes, the function sends those bytes to the print queue.
        // Returns true on success, false on failure.
        public static bool SendBytesToPrinter(string szPrinterName, IntPtr pBytes, Int32 dwCount)
        {
            Int32 dwError = 0, dwWritten = 0;
            IntPtr hPrinter = new IntPtr(0);
            DOCINFOA di = new DOCINFOA();
            bool bSuccess = false; // Assume failure unless you specifically succeed.

            di.pDocName = "Ticket de Venta";//Este es el nombre con el que guarda el archivo en caso de no imprimir a la impresora fisica.
            di.pDataType = "RAW";//de tipo texto plano

            // Open the printer.
            if (OpenPrinter(szPrinterName.Normalize(), out hPrinter, IntPtr.Zero))
            {
                // Start a document.
                if (StartDocPrinter(hPrinter, 1, di))
                {
                    // Start a page.
                    if (StartPagePrinter(hPrinter))
                    {
                        // Write your bytes.
                        bSuccess = WritePrinter(hPrinter, pBytes, dwCount, out dwWritten);
                        EndPagePrinter(hPrinter);
                    }
                    EndDocPrinter(hPrinter);
                }
                ClosePrinter(hPrinter);
            }
            // If you did not succeed, GetLastError may give more information
            // about why not.
            if (bSuccess == false)
            {
                dwError = Marshal.GetLastWin32Error();
            }
            return bSuccess;
        }

        public static bool SendStringToPrinter(string szPrinterName, string szString)
        {
            IntPtr pBytes;
            Int32 dwCount;
            // How many characters are in the string?
            dwCount = szString.Length;
            // Assume that the printer is expecting ANSI text, and then convert
            // the string to ANSI text.
            pBytes = Marshal.StringToCoTaskMemAnsi(szString);
            // Send the converted ANSI string to the printer.
            SendBytesToPrinter(szPrinterName, pBytes, dwCount);
            Marshal.FreeCoTaskMem(pBytes);
            return true;
        }
    }
}