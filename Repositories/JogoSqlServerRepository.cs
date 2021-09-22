using CatalogoJogosAPI.Entities;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;

namespace CatalogoJogosAPI.Repositories
{
    public class JogoSqlServerRepository : IJogoRepository
    {
        private readonly MySqlConnection sqlConnection;

        public JogoSqlServerRepository(IConfiguration configuration)
        {
            sqlConnection = new MySqlConnection(configuration.GetConnectionString("Default"));
        }
        public async Task<List<Jogo>> Obter(int pagina, int quantidade)
        {
            {
                var jogos = new List<Jogo>();

                var comando = $"select * from Jogos order by id LIMIT {quantidade} OFFSET {((pagina - 1) * quantidade)} ";

                await sqlConnection.OpenAsync();
                MySqlCommand sqlCommand = new MySqlCommand(comando, sqlConnection);
                DbDataReader sqlDataReader = await sqlCommand.ExecuteReaderAsync();

                while (sqlDataReader.Read())
                {
                    jogos.Add(new Jogo
                    {
                        Id = (Guid)sqlDataReader["Id"],
                        Nome = (string)sqlDataReader["Nome"],
                        Produtora = (string)sqlDataReader["Produtora"],
                        Preco = (double)sqlDataReader["Preco"]
                    });
                }

                await sqlConnection.CloseAsync();

                return jogos;
            }
        }
        public async Task<Jogo> Obter(Guid id)
        {
            Jogo jogo = null;

            var comando = $"select * from Jogos where Id = '{id}'";

            await sqlConnection.OpenAsync();
            MySqlCommand sqlCommand = new MySqlCommand(comando, sqlConnection);
            DbDataReader sqlDataReader = await sqlCommand.ExecuteReaderAsync();

            while (sqlDataReader.Read())
            {
                jogo = new Jogo
                {
                    Id = (Guid)sqlDataReader["Id"],
                    Nome = (string)sqlDataReader["Nome"],
                    Produtora = (string)sqlDataReader["Produtora"],
                    Preco = (double)sqlDataReader["Preco"]
                };
            }

            await sqlConnection.CloseAsync();

            return jogo;
        }
        public async Task<List<Jogo>> Obter(string nome, string produtora)
        {
            var jogos = new List<Jogo>();

            var comando = $"select * from Jogos where Nome = '{nome}' and Produtora = '{produtora}'";

            await sqlConnection.OpenAsync();
            MySqlCommand sqlCommand = new MySqlCommand(comando, sqlConnection);
            DbDataReader sqlDataReader = await sqlCommand.ExecuteReaderAsync();

            while (sqlDataReader.Read())
            {
                jogos.Add(new Jogo
                {
                    Id = (Guid)sqlDataReader["Id"],
                    Nome = (string)sqlDataReader["Nome"],
                    Produtora = (string)sqlDataReader["Produtora"],
                    Preco = (double)sqlDataReader["Preco"]
                });
            }

            await sqlConnection.CloseAsync();

            return jogos;
        }
        public async Task Inserir(Jogo jogo)
        {
            var comando = $"insert Jogos (Id, Nome, Produtora, Preco) values ('{jogo.Id}', '{jogo.Nome}', '{jogo.Produtora}', {jogo.Preco.ToString().Replace(",", ".")})";

            await sqlConnection.OpenAsync();
            MySqlCommand sqlCommand = new MySqlCommand(comando, sqlConnection);
            sqlCommand.ExecuteNonQuery();
            await sqlConnection.CloseAsync();
        }
        public async Task Atualizar(Jogo jogo)
        {
            var comando = $"update Jogos set Nome = '{jogo.Nome}', Produtora = '{jogo.Produtora}', Preco = {jogo.Preco.ToString().Replace(",", ".")} where Id = '{jogo.Id}'";

            await sqlConnection.OpenAsync();
            MySqlCommand sqlCommand = new MySqlCommand(comando, sqlConnection);
            sqlCommand.ExecuteNonQuery();
            await sqlConnection.CloseAsync();
        }
        public async Task Remover(Guid id)
        {
            var comando = $"delete from Jogos where Id = '{id}'";

            await sqlConnection.OpenAsync();
            MySqlCommand sqlCommand = new MySqlCommand(comando, sqlConnection);
            sqlCommand.ExecuteNonQuery();
            await sqlConnection.CloseAsync();
        }
        public void Dispose()
        {
            sqlConnection?.Close();
            sqlConnection?.Dispose();
        }
    }
}
