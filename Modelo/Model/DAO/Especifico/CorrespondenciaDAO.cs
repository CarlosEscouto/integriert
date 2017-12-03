using Model.Entity;
using System.Collections.Generic;
using System;
using Model.DAO.Generico;
using System.Data.SqlClient;

namespace Model.DAO.Especifico
{
	public class CorrespondenciaDAO
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

        public bool cadastra(Correspondencia correspondencia)
        {
            query = null;
            try
            {       
                query = "INSERT INTO CORRESPONDENCIA (DESCRICAO, ID_UNIDADE, DT_ENTRADA, DT_SAIDA, ID_PESSOA, STS_ATIVO, OBS_CANC) VALUES ('"
                        + correspondencia.descCorrespondencia + "', " 
                        + (correspondencia.unidade.id_unidade).ToString() + ", '"
                        + (correspondencia.dtEntrada).ToString() + "', NULL, NULL, 1, NULL);";
                return true;
            }

            catch (Exception ex)
            {
                return false;
                throw ex;
            }
        }

        public List<Correspondencia> buscaPorUnidade(int unidade)
		{
            query = null;
            List<Correspondencia> lstCorrespondencia = new List<Correspondencia>();
            try
            {
                query = "SELECT C.DESCRICAO, U.IDENTIFICACAO, C.DT_ENTRADA, C.DT_SAIDA, P.ID_PESSOA, C.OBS_CANC FROM CORRESPONDENCIA AS C "
                        + " INNER JOIN UNIDADE AS U ON C.ID_UNIDADE = U.ID_UNIDADE "
                        + " LEFT OUTER JOIN PESSOA AS P ON C.ID_PESSOA = P.ID_PESSOA "
                        + " WHERE C.ID_UNIDADE = " + unidade.ToString()
                        + " AND C.STS_ATIVO = 1;";
                lstCorrespondencia = setarObjeto(banco.MetodoSelect(query));
            }

            catch (Exception ex)
            {
                throw ex;
            }

            return lstCorrespondencia;
        }		

		public List<Correspondencia> buscaPorData(DateTime dtEntrada, DateTime dtSaida)
		{
            query = null;
            List<Correspondencia> lstCorrespondencia = new List<Correspondencia>();
            try
            {
                query = "SELECT C.DESCRICAO, U.IDENTIFICACAO, C.DT_ENTRADA, C.DT_SAIDA, P.ID_PESSOA, C.OBS_CANC FROM CORRESPONDENCIA AS C "
                        + " INNER JOIN UNIDADE AS U ON C.ID_UNIDADE = U.ID_UNIDADE "
                        + " LEFT OUTER JOIN PESSOA AS P ON C.ID_PESSOA = P.ID_PESSOA "
                        + " WHERE C.DT_ENTRADA = " + (dtEntrada).ToShortDateString() + " AND C.DT_SAIDA = " + (dtSaida).ToShortDateString()
                        + " AND C.STS_ATIVO = 1;";
                lstCorrespondencia = setarObjeto(banco.MetodoSelect(query));
            }

            catch (Exception ex)
            {
                throw ex;
            }

            return lstCorrespondencia;
        }

		public List<Correspondencia> busca()
		{
            query = null;
            List<Correspondencia> lstCorrespondencia = new List<Correspondencia>();
            try
            {
                query = "SELECT C.DESCRICAO, U.IDENTIFICACAO, C.DT_ENTRADA, C.DT_SAIDA, P.ID_PESSOA, C.OBS_CANC FROM CORRESPONDENCIA AS C "
                        + " INNER JOIN UNIDADE AS U ON C.ID_UNIDADE = U.ID_UNIDADE "
                        + " LEFT OUTER JOIN PESSOA AS P ON C.ID_PESSOA = P.ID_PESSOA "
                        + " WHERE C.STS_ATIVO = 1;";
                lstCorrespondencia = setarObjeto(banco.MetodoSelect(query));
            }

            catch (Exception ex)
            {
                throw ex;
            }

            return lstCorrespondencia;
        }

        public bool altera(Correspondencia correspondencia)
        {
            query = null;
            try
            {
                query = "UPDATE CORRESPONDENCIA SET " 
                        + "DESCRICAO = '" + correspondencia.descCorrespondencia 
                        + "' WHERE ID_CORRESPONDENCIA = " + correspondencia.id_correspondencia.ToString();
                banco.MetodoNaoQuery(query);
                return true;
            }

            catch (Exception ex)
            {
                return false;
                throw ex;
            }
        }

		public bool remove(int id, string obs_canc) 
		{
            query = null;
            try
            {
                query = "UPDATE CORRESPONDENCIA SET STS_ATIVO = 0, OBS_CANC = " + obs_canc 
                        + " WHERE ID_CORRESPONDENCIA = " + id.ToString();
                banco.MetodoNaoQuery(query);
                return true;
            }

            catch (Exception ex)
            {
                return false;
                throw ex;
            }
        }

        public bool retirada(int id, int dt_saida, int id_pessoa)
        {
            query = null;
            try
            {
                query = "UPDATE CORRESPONDENCIA SET DT_SAIDA = " + dt_saida + ", ID_PESSOA = " + id_pessoa 
                        + " WHERE ID_CORRESPONDENCIA = " + (id).ToString();
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

        public List<Correspondencia> setarObjeto(SqlDataReader dr)
        {
            List<Correspondencia> lstCorresp = new List<Correspondencia>();

            try
            {
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        Correspondencia obj = new Correspondencia();
                        obj.id_correspondencia = Convert.ToInt32(dr["ID_CORRESPONDENCIA"].ToString());
                        obj.descCorrespondencia = Convert.ToString(dr["DESCRICAO"].ToString());
                        obj.dtEntrada = Convert.ToDateTime(dr["DT_ENTRADA"].ToString());
                        obj.dtSaida = Convert.ToDateTime(dr["DT_SAIDA"].ToString());
                        obj.ativo = Convert.ToInt32(dr["STS_ATIVO"].ToString());
                        obj.obsCancelamento = Convert.ToString(dr["OBS_CANC"].ToString());

                        obj.unidade.id_unidade = Convert.ToInt32(dr["ID_UNIDADE"].ToString());
                        obj.unidade.identificacao = Convert.ToString(dr["IDENTIFICACAO"].ToString());

                        obj.responsavelRetirada.id_pessoa = Convert.ToInt32(dr["ID_PESSOA"].ToString());

                        lstCorresp.Add(obj);
                    }
                }
            }

            catch (Exception ex)
            {
                dr.Dispose();
                throw ex;
            }

            return lstCorresp;
        }

        #endregion
	}

}

