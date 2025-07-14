using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1.Models
{
    public enum FormaDePagamento
    {
        Dinheiro,
        Cartao,
        Boleto
    }

    public enum StatusPedido
    { 
        Pendente,
        Pago,
        Enviado,
        Recebido
    }
}
