using Model.Entity;
using System.Collections.Generic;
using System.Data.SqlClient;
using Model.DAO.Generico;
using System.Data;
using System;

namespace Model.DAO.Especifico
{
	public class LoginDAO
	{

        #region Observa��es

        //Por padr�o, todas as buscas ser�o WHERE STS_ATIVO = 1, exceto a verifica��o se j� existe o cadastro.
        //O prof Cassiano orientou a implementar o setarObjeto() dessa forma que foi feita, pq todas as classes precisam, com parametros e objetos diferentes. N�o vale o trampo de abstrair.

        #endregion

        #region Objetos

        dbBancos banco = new dbBancos();
        string query = null;

        #endregion

        #region CRUD

        Login obj = new Login();
        public bool cadastra(Login login)
		{
            query = null;

            try
            {
                query = "INSERT INTO LOGIN (EMAIL, SENHA, NIVEL_ACESSO, ID_PESSOA, STS_ATIVO) VALUES ('" +
                        login.login + "', '" + 
                        login.senha + "', " +
                        login.permissao.ToString() + ", " + 
                        login.pessoa.id_pessoa.ToString() + ", 1);";

                banco.MetodoNaoQuery(query);
                return true;
            }

            catch (Exception ex)
            {
                return false;
                throw ex;
            }
        }

		public List<Login> buscaLogin(string login, string senha)
		{
            query = null;
            List<Login> lstLogin = new List<Login>();
            try
            {
                query = "SELECT A.TITULO, A.DESCRICAO, A.DT_AVISO, C.NOME FROM AVISO AS A "
                        + " INNER JOIN CONDOMINIO AS C ON A.ID_COND = C.ID_COND "
                        + " WHERE A.STS_ATIVO = 1;";
                lstLogin = setarObjeto(banco.MetodoSelect(query));
            }

            catch (Exception ex)
            {
                throw ex;
            }

            return lstLogin;
        }

        public bool altera(Login login)
        {
            query = null;
            try
            {
                query = "UPDATE LOGIN SET EMAIL = '" + login.login +
                        "', SENHA = '" + login.senha + 
                        "', NIVEL_ACESSO = '" + login.permissao + 
                        "' WHERE ID_LOGIN = " + login.id_login.ToString() + ";";
                banco.MetodoNaoQuery(query);
                return true;
            }

            catch (Exception ex)
            {
                return false;
                throw ex;
            }
        }

        public bool remove(int id)
		{
            query = null;
            try
            {
                query = "UPDATE LOGIN SET STS_ATIVO = 0 WHERE ID_LOGIN = " + id.ToString() + ";";
                banco.MetodoNaoQuery(query); 
                return true;
            }

            catch (Exception ex)
            {
                return false;
                throw ex;
            }
        }

        #endregion

        #region M�todos

        public List<Login> setarObjeto(SqlDataReader dr)
        {
            List<Login> lstLogin = new List<Login>();
            try
            {
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        Login obj = new Login();
                        obj.id_login = Convert.ToInt32(dr["ID_LOGIN"].ToString());
                        obj.login = Convert.ToString(dr["EMAIL"].ToString());
                        obj.senha = Convert.ToString(dr["SENHA"].ToString());
                        obj.permissao = Convert.ToInt32(dr["NIVEL_ACESSO"].ToString());
                        obj.ativo = Convert.ToInt32(dr["STS_ATIVO"].ToString());

                        obj.pessoa.id_pessoa = Convert.ToInt32(dr["ID_PESSOA"].ToString());

                        lstLogin.Add(obj);
                    }
                }
            }

            catch (Exception ex)
            {
                dr.Dispose();
                throw ex;
            }

            return lstLogin;
        }

        #endregion
    }

}

