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
        public ActionResult Index()
        {
            // db.Agentes.ToList() ---> Select* From Agentes
            //enviar para a View a lista de todos os agentes da base de dados
            return View(db.Agentes.ToList());
        }

        // GET: Agentes/Details/5
        public ActionResult Details(int? id)
        {
            // se se escrever int? e possivel
            // nao fornecero valor para o id e nao há erro

            //proteçao para o caso de nao ter sido fornecido um ID valido
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //procura na BD o agente cujo o id foi fornecido
            Agentes agentes = db.Agentes.Find(id);

            // protecao para o caso de nao ter sido encontrado qq agente com id fornecido
            if (agentes == null)
            {
                return HttpNotFound();
            }
            //entrega a view os dados do Agente encontrado
            return View(agentes);
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
            int idNovoAgente = db.Agentes.Max(a => a.ID) + 1;
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
                //adiciona o novo Agente a bd
                db.Agentes.Add(agente);
                //faz commit as alteraçoes
                db.SaveChanges();
                //se tudo correr bem, redireciona para a pagina de Index
                return RedirectToAction("Index");
            }
            //se houver um erro representa os dados do Agente na view
            return View(agente);
        }

        // GET: Agentes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Agentes agentes = db.Agentes.Find(id);
            if (agentes == null)
            {
                return HttpNotFound();
            }
            return View(agentes);
        }

        // POST: Agentes/Edit/5
        // Para se proteger de mais ataques, ative as propriedades específicas a que você quer se conectar. Para 
        // obter mais detalhes, consulte https://go.microsoft.com/fwlink/?LinkId=317598.
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
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Agentes agentes = db.Agentes.Find(id);
            if (agentes == null)
            {
                return HttpNotFound();
            }
            return View(agentes);
        }

        // POST: Agentes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Agentes agentes = db.Agentes.Find(id);
            //remove agentes da bd
            db.Agentes.Remove(agentes);
            //faz commmit 
            db.SaveChanges();
            return RedirectToAction("Index");
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
