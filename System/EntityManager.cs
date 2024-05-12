using System.Collections.Generic;
using Higurashi_Game.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Higurashi_Game.System;

public class EntityManager
{
    private readonly List<IGameEntity> _entities = new List<IGameEntity>();
    private readonly List<IGameEntity> _entitiesToAdd = new List<IGameEntity>();
    private readonly List<IGameEntity> _entitiesToRemove = new List<IGameEntity>();

    public void Update(GameTime gameTime)
    {
        foreach (IGameEntity entity in _entities)
        {
            entity.Update(gameTime);
        }
        
        foreach (IGameEntity entity in _entitiesToAdd)
        {
            _entities.Add(entity);
        }
        
        foreach (IGameEntity entity in _entitiesToRemove)
        {
            _entities.Remove(entity);
        }
        
        _entitiesToAdd.Clear();
        _entitiesToRemove.Clear();
    }
    
    public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
    {
        foreach (IGameEntity entity in _entities)
        {
            entity.Draw(spriteBatch, gameTime);
        }
    }

    public void AddEntity(IGameEntity entity)
    {
        _entitiesToAdd.Add(entity);
    }
    
    public void RemoveEntity(IGameEntity entity)
    {
        _entitiesToRemove.Add(entity);
    }

    public void Clear()
    {
        _entitiesToRemove.AddRange(_entities);
    }
}