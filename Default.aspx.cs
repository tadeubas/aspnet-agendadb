﻿using System;
using System.Collections.Generic;
//using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.Odbc;
using System.Configuration;

public partial class _Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Cria lista para o objeto
            List<agenda> msg = new List<agenda>();

            // Adiciona dados ao objeto
            try
            {
                using (OdbcConnection connection = new OdbcConnection(ConfigurationManager.ConnectionStrings["MySQLServerConnStr"].ConnectionString))
                {
                    connection.Open();
                    using (OdbcCommand command = new OdbcCommand("SELECT * FROM agenda", connection))
                    using (OdbcDataReader dr = command.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            msg.Add(new agenda(Int32.Parse(dr["id"].ToString()), dr["nome"].ToString(), dr["telefone"].ToString()));
                        }
                        dr.Close();
                    }
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                Response.Write("An error occured: " + ex.Message);
            }

            // Adiciona objeto a sessão
            Session["registros"] = msg;

            // Cria pagina no método HTTP GET
            if (Request.HttpMethod.Equals("GET"))
                doGet();

        }

        private void doGet()
        {
            // Cria estilo CSS
            Response.Write("<head>");
            Response.Write("<style>");
            Response.Write("td, th {");
            Response.Write("border: none;");
            Response.Write("background-color: #dddddd;");
            Response.Write("padding: 5px;");
            Response.Write("width: 200px; }");
            Response.Write("</style>");
            Response.Write("</head>");

            // Titulo da pagina
            Response.Write("<h1>Agenda Telefônica</h1>");

            // Cria tabela
            Response.Write("<table>");
            Response.Write("<tr>");
            Response.Write("<th>ID:</th>");
            Response.Write("<th>Nome:</th>");
            Response.Write("<th>Telefone:</th>");
            Response.Write("<th>Editar / Deletar:</th>");
            Response.Write("</tr>");

            // Recupera objeto da sessão
            List<agenda> registros = (List<agenda>)Session["registros"];

            // Itera objeto
            foreach (agenda reg in registros)
            {
                Response.Write("<tr>");
                Response.Write("<td>" + reg.Id + "</td>");
                Response.Write("<td>" + reg.Nome + "</td>");
                Response.Write("<td>" + reg.Telefone + "</td>");

                // Link de edição
                Response.Write("<td><center>");
                Response.Write("<a href='atualiza_tel.aspx?identificador=" + reg.Id + "'>Editar</a> | ");
                Response.Write("<a href='deleta_tel.aspx?identificador=" + reg.Id + "'>Apagar</a>");
                Response.Write("</center></td>");
                Response.Write("</tr>");
            }

            Response.Write("</table>");

            // Define link para formulario de adicionar dados
            Response.Write("<P> <a href='add_tel.aspx'>Adicione um novo contato</a> </p>");
        }
    }