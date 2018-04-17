using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Multas_tB.Models;

namespace Multas_tB.Controllers
{
    public class AgentesController : Controller
    {
        //cria uma variavel que representa a Base de Dados
        private MultasDb db = new MultasDb();

        // GET: Agentes
        /// <summary>
        /// lista todos os agentes
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            // db.Agentes.ToList() ---> Select* From Agentes
            //enviar para a View a lista de todos os agentes da base de dados
            //obter a lista de todos os agentes
            var listaDeAgentes = db.Agentes.ToList().OrderBy(a=>a.Nome);
            return View(listaDeAgentes);
        }

        // GET: Agentes/Details/5
        public ActionResult Details(int? id)
        {
            // se se escrever int? e possivel
            // nao fornecero valor para o id e nao há erro

            //proteçao para o caso de nao ter sido fornecido um ID valido
            if (id == null)
            {
                //instrução original devolve erro qd não há ID logo não há pesquisa
                // return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                // redirecionar para uma pagina que nos controlamos
                return RedirectToAction("Index");
            }
            //procura na BD o agente cujo o id foi fornecido
            Agentes agente = db.Agentes.Find(id);

            // protecao para o caso de nao ter sido encontrado qq agente com id fornecido
            if (agente == null)
            {
                //o agente nao foi encontrado logo, gera-se msg de erro
                //return HttpNotFound();
                // redirecionar para uma pagina que nos controlamos
                return RedirectToAction("Index");
            }
            //entrega a view os dados do Agente encontrado
            return View(agente);
        }

        // GET: Agentes/Create
        public ActionResult Create()
        {
            //apresenta a view para se inserir um novo agente
            return View();
        }

        // POST: Agentes/Create
        // Para se proteger de mais ataques, ative as propriedades específicas a que você quer se conectar. Para 
        // obter mais detalhes, consulte https://go.microsoft.com/fwlink/?LinkId=317598.
        //anotador para HTTP Post
        [HttpPost]
        //anotador para protecao de roubo de identidade
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Nome,Esquadra")
            ] Agentes agente, HttpPostedFileBase uploadFotografia)
        {
            // escrever os dados de um novo agente na BD
            //especificar o nome do agente
            //testar se há registos na tabela dos Agentes
            //if (db.Agentes.Count() != 0){}
            //ou entao usar a istruçao try/catch
            int idNovoAgente = 0;
            try{
                idNovoAgente = db.Agentes.Max(a => a.ID) + 1;
            }
            catch (Exception){
                idNovoAgente = 1;
            }
            //guardar o id do novo agente
            agente.ID = idNovoAgente;
            //especificar (escolher) o nome do ficheiro
            string nomeImagem = "Agente_"+idNovoAgente+".jpg";
            //var auxiliar
            string path="";
            //validar se a imagem foi fornecida
            if(uploadFotografia != null)
            {
                //o ficheiro foi fornecido
                //validar se o que foi fornecido e uma imagem---> fazer em casa
                //formatar o tamanho da imagem 
                //criar o caminho completo até ao sitio onde o ficheiro sera guardado
                path = Path.Combine(Server.MapPath("~/imagens/"), nomeImagem);
                //guardar o nome do ficheiro na BD
                agente.Fotografia = nomeImagem;
            }
           else
            {
                //não foi fornecido qualquer ficheiro
                ModelState.AddModelError("", "Não foi fornecida imagem");
                //devolver o controlo a view
                return View(agente);
            }
            //escrever o ficheiro com a fotografia no disco rigido na pasta 'imagens'
            //guardar o nome escolhido na BD


            //ModelState.IsValid -> confrota os dados fornecidos da view com as exigencias do modelo
            if (ModelState.IsValid)
            {
                try
                {
                    //adiciona o novo Agente a bd
                    db.Agentes.Add(agente);
                    //faz commit as alteraçoes
                    db.SaveChanges();
                    //escrever o ficheiro com a fotografia no disco rigido na pasta
                    uploadFotografia.SaveAs(path);


                    //se tudo correr bem, redireciona para a pagina de Index
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Houve um erro com a criação do Agente");

                    /// se existir uma classe chamada 'ERRO.cs' iremos nela registar
                    /// os dados do erro, 
                    /// -criar um objeto desta classe 
                    /// -atribuir a esse objeto os dados do erro
                    /// - nome do controller, nome do metodo, data + hora, mensagem 
                    /// -dados que se tentavam inserir 
                    /// -outros dados considerados relevantes
                    /// - guardar o obejeto na BD
                    /// - notificar um gestor do sistema, por email,ou por outro meio do
                    /// erro e dos seus dados



                }
            }
            //se houver um erro representa os dados do Agente na view
            return View(agente);
        }

        // GET: Agentes/Edit/5
        public ActionResult Edit(int? id)
        {
            // se se escrever int? e possivel
            // nao fornecero valor para o id e nao há erro

            //proteçao para o caso de nao ter sido fornecido um ID valido
            if (id == null)
            {
                //instrução original devolve erro qd não há ID logo não há pesquisa
                // return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                // redirecionar para uma pagina que nos controlamos
                return RedirectToAction("Index");
            }
            //procura na BD o agente cujo o id foi fornecido
            Agentes agente = db.Agentes.Find(id);

            // protecao para o caso de nao ter sido encontrado qq agente com id fornecido
            if (agente == null)
            {
                //o agente nao foi encontrado logo, gera-se msg de erro
                //return HttpNotFound();
                // redirecionar para uma pagina que nos controlamos
                return RedirectToAction("Index");
            }
            //entrega a view os dados do Agente encontrado
            return View(agente);
        }

        // POST: Agentes/Edit/5
        // Para se proteger de mais ataques, ative as propriedades específicas a que você quer se conectar. Para 
        // obter mais detalhes, consulte https://go.microsoft.com/fwlink/?LinkId=317598.
        /// <summary>
        /// 
        /// </summary>
        /// <param name="agentes"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Nome,Esquadra,Fotografia")] Agentes agentes)
        {
            if (ModelState.IsValid)
            {
                //neste caso ja existe um agente apenas quero Editar os seus dados
                db.Entry(agentes).State = EntityState.Modified;
                //efetua commit
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(agentes);
        }

        // GET: Agentes/Delete/5
        /// <summary>
        /// apresenta na view os dados de um agente 
        /// com vista a sua eventual eliminacao
        /// </summary>
        /// <param name="id">identificador do agente</param>
        /// <returns></returns>
        public ActionResult Delete(int? id)
        {
            //verifica se o ID fornecido é válido
            if (id == null)
            {
                return RedirectToAction("Index");
            }
            //pesquisa pelo agente cujo ID foi fornecido
            Agentes agente = db.Agentes.Find(id);

            // verifica se o agente foi encontrado
            if (agente == null)
            {
                // agente nao existe redirecciona para a pagina inicial
                return RedirectToAction("Index");
            }
            return View(agente);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // POST: Agentes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Agentes agente = db.Agentes.Find(id);

            try
            {
                
                //remove agentes da bd
                db.Agentes.Remove(agente);
                //faz commmit 
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", string.Format("Não é possivel apagar o agente nº {0}-{1} porque há multas associadas a ele", id,agente.Nome));
            }
            //se cheguei aqui e pq houve um problema
            //devolvo os dados do Agente a view
            return View(agente);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
