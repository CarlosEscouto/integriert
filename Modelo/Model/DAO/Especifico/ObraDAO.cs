using Model.Entity;
using System.Collections.Generic;
using System;
using Model.DAO.Generico;
using System.Data.SqlClient;

namespace Model.DAO.Especifico
{
	public class ObraDAO
	{
        #region Observa��es

        //Por padr�o, todas as buscas ser�o WHERE STS_ATIVO = 1, exceto a verifica��o se j� existe o cadastro.
        //O prof Cassiano orientou a implementar o setarObjeto() dessa forma que foi feita, pq todas as classes precisam, com parametros e objetos diferentes. N�o vale o trampo de abstrair.
        // lstArea apenas recebe a outra lista pra nao sair do try catch;
        #endregion

        #region Objetos

        dbBancos banco = new dbBancos();
        string query = null;

        #endregion

        //List<Obra> lstObra = new List<Obra>();
        
        #region TIPO OBRA

        public bool cadastraTipoObra(Obra obra)
        {
            query = null;
            try
            {
                query = "INSERT INTO TIPO_OBRA (DESCRICAO, STS_ATIVO) VALUES ('"
                        + obra.desc_tipo + "', 1);";
                banco.MetodoNaoQuery(query);
                return true;
            }

            catch (Exception ex)
            {
                return false;
                throw ex;
            }
        }

        public List<Obra> buscaTipoObra()
        {
            query = null;
            List<Obra> lstObra = new List<Obra>();
            try
            {
                query = "SELECT * FROM TIPO_OBRA WHERE STS_ATIVO = 1 ORDER BY DESCRICAO;";
                lstObra = setarObjetoTipoObra(banco.MetodoSelect(query));
            }

            catch (Exception ex)
            {
                throw ex;
            }

            return lstObra;
        }

        public bool alteraTipoObra(Obra obra)
        {
            query = null;
            try
            {
                query = "UPDATE TIPO_OBRA SET DESCRICAO = '" + obra.desc_tipo 
                    + "' WHERE ID_TIPO_OBRA = " + obra.id_tipo_obra.ToString() + ";";

                banco.MetodoNaoQuery(query);
                return true;
            }

            catch (Exception ex)
            {
                return false;
                throw ex;
            }
        }

        public bool removeTipoObra(int id)
        {
            query = null;
            try
            {
                query = "UPDATE TIPO_OBRA SET STS_ATIVO = 0 WHERE ID_TIPO_OBRA = " + id.ToString() + ";";

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

        #region OBRA

        public bool cadastraObra(Obra obra)
		{
            query = null;
            try
            {
                query = "INSERT INTO OBRA (DESCRICAO, DT_INICIO, DT_PREVISAO_TERMINO, FINALIZADA, " +
                    "ID_AREA, ID_TIPO_OBRA, ID_COND, STS_ATIVO) VALUES ('" +
                    obra.descricao_obra + "', '" + 
                    obra.dt_inicio + "', '" +
                    obra.dt_previsao_termino + "', '" +
                    obra.finalizada.ToString() + "', " +
                    obra.area.id_area.ToString() + ", " +
                    obra.id_tipo_obra.ToString() + ", " +
                    obra.cond.id_cond.ToString() + ", 1);";

                banco.MetodoNaoQuery(query);
                return true;
            }

            catch (Exception ex)
            {
                return false;
                throw ex;
            }
        }

		//public List<Obra> buscaPorArea(Area area)
		//{
  //          query = null;
  //          List<Obra> lstObra = new List<Obra>();
  //          try
  //          {
  //              query = "SELECT * FROM OBRA WHERE NOME LIKE '%" + nome + "%' AND STS_ATIVO = 1 ORDER BY NOME;";
  //              lstObra = setarObjeto(banco.MetodoSelect(query));
  //          }

  //          catch (Exception ex)
  //          {
  //              throw ex;
  //          }

  //          return lstObra;
  //      }

		//public List<Obra> buscaPorTipo(string tipo)
		//{
  //          return lstObra;
  //      }	
		
		//public List<Obra> buscaPorData(DateTime data)
		//{
  //          return lstObra;
  //      }

		public List<Obra> buscaPorAbertas()
		{
            query = null;
            List<Obra> lstObra = new List<Obra>();
            try
            {
                query = "SELECT * FROM OBRA WHERE STS_ATIVO = 1 AND FINALIZADA = 0 " +
                        " ORDER BY DT_INICIO DESC;";
                lstObra = setarObjeto(banco.MetodoSelect(query));
            }

            catch (Exception ex)
            {
                throw ex;
            }

            return lstObra;
        }	

		public List<Obra> buscaPorFechadas()
		{
            query = null;
            List<Obra> lstObra = new List<Obra>();
            try
            {
                query = "SELECT * FROM OBRA WHERE STS_ATIVO = 1 AND FINALIZADA = 1 " +
                        " ORDER BY DT_INICIO DESC;";
                lstObra = setarObjeto(banco.MetodoSelect(query));
            }

            catch (Exception ex)
            {
                throw ex;
            }

            return lstObra;
        }						

		public List<Obra> busca()
		{
            query = null;
            List<Obra> lstObra = new List<Obra>();
            try
            {
                query = "SELECT * FROM OBRA WHERE STS_ATIVO = 1 ORDER BY DT_INICIO DESC;";
                lstObra = setarObjeto(banco.MetodoSelect(query));
            }

            catch (Exception ex)
            {
                throw ex;
            }

            return lstObra;
        }

        public bool altera(string dt_term, bool finalizada, int id)
        {
            query = null;
            try
            {
                query = "UPDATE OBRA SET DT_TERMINO = '" + dt_term
                        + "', FINALIZADA = '"+ finalizada +"' WHERE ID_OBRA = " 
                        + id.ToString() + ";";

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
                query = "UPDATE OBRA SET STS_ATIVO = 0 WHERE ID_OBRA = " + id.ToString() + ";";
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

        #region TERCEIRO OBRA

        public bool cadastraTerceiroObra(Terceiro terceiro, Obra obra)
        {
            query = null;
            try
            {

                query = "INSERT INTO TERCEIRO_OBRA (ID_TERCEIRO, ID_OBRA, STS_ATIVO) VALUES (" +
                        terceiro.id_terceiro.ToString() + ", " + obra.id_obra.ToString() + ", 1);";

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

        public List<Obra> setarObjeto(SqlDataReader dr)
        {
            List<Obra> lstObra = new List<Obra>();
            try
            {
                if (dr.HasRows)
                {
                    while (dr.Read() == true)
                    {
                        Obra obj = new Obra();
                        obj.id_obra = Convert.ToInt32(dr["ID_OBRA"].ToString());
                        obj.descricao_obra = Convert.ToString(dr["DESCRICAO"].ToString());
                        obj.finalizada = Convert.ToBoolean(dr["FINALIZADA"].ToString());
                        obj.dt_inicio = Convert.ToString(dr["DT_INICIO"].ToString());
                        obj.dt_previsao_termino = Convert.ToString(dr["DT_PREVISAO_TERMINO"].ToString());
                        obj.dt_termino = Convert.ToDateTime(dr["DT_TERMINO"].ToString());
                        obj.ativo = Convert.ToBoolean(dr["STS_ATIVO"].ToString());
                        
                        obj.id_tipo_obra = Convert.ToInt32(dr["ID_TIPO_OBRA"].ToString());
                        obj.desc_tipo = Convert.ToString(dr["ID_AREA"].ToString());

                        obj.area = new Area();
                        obj.area.id_area = Convert.ToInt32(dr["ID_AREA"].ToString());

                        lstObra.Add(obj);
                    }
                }
            }

            catch (Exception ex)
            {
                dr.Dispose();
                throw ex;
            }

            return lstObra;
        }

        public List<Obra> setarObjetoTipoObra(SqlDataReader dr)
        {
            List<Obra> lstObra = new List<Obra>();
            try
            {
                if (dr.HasRows)
                {
                    while (dr.Read() == true)
                    {
                        Obra obj = new Obra();

                        obj.id_tipo_obra = Convert.ToInt32(dr["ID_TIPO_OBRA"].ToString());
                        obj.desc_tipo = Convert.ToString(dr["DESCRICAO"].ToString());
                        obj.ativo = Convert.ToBoolean(dr["STS_ATIVO"].ToString());
                        lstObra.Add(obj);
                    }
                }
            }

            catch (Exception ex)
            {
                dr.Dispose();
                throw ex;
            }

            return lstObra;
        }

        #endregion
    }

}

