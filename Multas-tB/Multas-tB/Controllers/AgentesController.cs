using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
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
        public ActionResult Create([Bind(Include = "Nome,Esquadra,Fotografia")] Agentes agentes)
        {
            // escrever os dados de um novo agente na BD

            //ModelState.IsValid -> confrota ks dados fornecidos da view com as exigencias do modelo
            if (ModelState.IsValid)
            {
                //adiciona o novo Agente a bd
                db.Agentes.Add(agentes);
                //faz commit as alteraçoes
                db.SaveChanges();
                //se tudo correr bem, redireciona para a pagina de Index
                return RedirectToAction("Index");
            }
            //se houver um erro representa os dados do Agente na view
            return View(agentes);
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
