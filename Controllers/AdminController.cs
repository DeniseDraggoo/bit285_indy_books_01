﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IndyBooks.Models;
using IndyBooks.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace IndyBooks.Controllers
{
    public class AdminController : Controller
    {
        private IndyBooksDataContext _db;
        public AdminController(IndyBooksDataContext db) { _db = db; }

        public IActionResult Search()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Search(SearchViewModel search)
        {
            IQueryable<Book> foundBooks = _db.Books; // start with entire collection

            //Filter the collection using each non-empty Field as noted
            if (search.Title != null)
            {
                //Filter the collection by Title which "contains" string 
                //(Note: searchBook is the info from the form)
                // TODO: order the results by Title
                foundBooks = foundBooks
                    .OrderBy (b => b.Title)         
                    .Where(b => b.Title.Contains(search.Title));
            }

            //TODO: Add logic to filter the collection by last part of the Author's Name, if given
            // (HINT: consider the EndsWith() method, also you will need to adjust the View and ViewModel)
            if (search.Author != null)
            {
                foundBooks = foundBooks
                    .OrderBy(b => b.Title)
                    .Where(b => b.Author.EndsWith(search.Author));
            }

            //TODO: Filter the collection by price between a low and high value, if given
            //       order results by descending price 
            // (Note: you will need to adjust the ViewModel and View to add search fields)
            if (search.MinPrice != 0 && search.MaxPrice != 0)
            {
                foundBooks = foundBooks
                    .OrderByDescending(b => b.Price)
                    .Where (b => b.Price >= search.MinPrice && b.Price <= search.MaxPrice);
                
            }
            //Check if one of the prices is not entered
            if (search.MaxPrice == 0)
            {
                foundBooks = foundBooks
                                    .OrderByDescending(b => b.Price)
                                    .Where(b => b.Price >= search.MinPrice);
            }
            //If the minum price is not given, print all book that are less than or equal to the maximum price
            if (search.MinPrice == 0)
            {
                foundBooks = foundBooks
                                    .OrderByDescending(b => b.Price)
                                    .Where(b => b.Price <= search.MaxPrice);
            }

            return View("SearchResults", foundBooks);
        }
    }
}
