﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DevSteamAPI.Data;
using DevSteamAPI.Models;

namespace DevSteamAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemCarrinhosController : ControllerBase
    {
        private readonly DevSteamAPIContext _context;

        public ItemCarrinhosController(DevSteamAPIContext context)
        {
            _context = context;
        }

        // GET: api/ItemCarrinhos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ItemCarrinho>>> GetItemCarrinho()
        {
            return await _context.ItemCarrinho.ToListAsync();
        }

        // GET: api/ItemCarrinhos/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ItemCarrinho>> GetItemCarrinho(Guid id)
        {
            var itemCarrinho = await _context.ItemCarrinho.FindAsync(id);

            if (itemCarrinho == null)
            {
                return NotFound();
            }

            return itemCarrinho;
        }

        // PUT: api/ItemCarrinhos/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutItemCarrinho(Guid id, ItemCarrinho itemCarrinho)
        {
            if (id != itemCarrinho.ItemCarrinhoId)
            {
                return BadRequest();
            }

            _context.Entry(itemCarrinho).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ItemCarrinhoExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/ItemCarrinhos
        [HttpPost]
        public async Task<ActionResult<ItemCarrinho>> PostItemCarrinho(ItemCarrinho itemCarrinho)
        {
            _context.ItemCarrinho.Add(itemCarrinho);

            var carrinho = await _context.Carrinho.FindAsync(itemCarrinho.CarrinhoId);
            if (carrinho != null)
            {
                carrinho.Total += itemCarrinho.Quantidade * itemCarrinho.Valor;
                _context.Entry(carrinho).State = EntityState.Modified;
            }

            await _context.SaveChangesAsync();

            return CreatedAtAction("GetItemCarrinho", new { id = itemCarrinho.ItemCarrinhoId }, itemCarrinho);
        }

        // DELETE: api/ItemCarrinhos/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteItemCarrinho(Guid id)
        {
            var itemCarrinho = await _context.ItemCarrinho.FindAsync(id);
            if (itemCarrinho == null)
            {
                return NotFound();
            }

            var carrinho = await _context.Carrinho.FindAsync(itemCarrinho.CarrinhoId);
            if (carrinho != null)
            {
                carrinho.Total -= itemCarrinho.Quantidade * itemCarrinho.Valor;
                _context.Entry(carrinho).State = EntityState.Modified;
            }

            _context.ItemCarrinho.Remove(itemCarrinho);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // POST: api/ItemCarrinhos/AddItemAndUpdateTotal
        [HttpPost("AddItemAndUpdateTotal")]
        public async Task<ActionResult<ItemCarrinho>> AddItemAndUpdateTotal(ItemCarrinho itemCarrinho)
        {
            _context.ItemCarrinho.Add(itemCarrinho);

            var carrinho = await _context.Carrinho.FindAsync(itemCarrinho.CarrinhoId);
            if (carrinho != null)
            {
                carrinho.Total += itemCarrinho.Quantidade * itemCarrinho.Valor;
                _context.Entry(carrinho).State = EntityState.Modified;
            }

            await _context.SaveChangesAsync();

            return CreatedAtAction("GetItemCarrinho", new { id = itemCarrinho.ItemCarrinhoId }, itemCarrinho);
        }

        private bool ItemCarrinhoExists(Guid id)
        {
            return _context.ItemCarrinho.Any(e => e.ItemCarrinhoId == id);
        }
    }
}
