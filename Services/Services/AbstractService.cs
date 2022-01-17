﻿using AutoMapper;
using Data.Repositories;
using Core.Models;
using Services.Intrefaces;
using Services.ModelsDTO;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services.Services {
    public abstract class AbstractService<TModel, TDTO> : IService<TModel, TDTO> 
        where TModel : AbstractModel where TDTO : AbstractDTO{

        protected readonly IRepository<TModel> Repository;
        protected readonly IMapper Mapper;

        protected AbstractService(IRepository<TModel> repository, IMapper mapper) {
            Repository = repository;
            Mapper = mapper;
        }

        public async Task<IEnumerable<TDTO>> GetAllItemsAsync() {
            var items = await Repository.GetEntitiesListAsync();
            return Mapper.Map<IEnumerable<TDTO>>(items);
        }

        public async Task<TDTO> GetAsync(int? id) {
            if (id is null) {
                throw new Exception("No such entity in database");
            }

            var item = await Repository.GetEntityAsync((int)id);
            if (item is null) {
                throw new Exception("No such entity in database");
            }
            return Mapper.Map<TDTO>(item);

        }
        public async Task CreateAsync(TDTO itemDTO) {
            var item = Mapper.Map<TModel>(itemDTO);
            await Repository.CreateAsync(item);
        }
        public async Task UpdateAsync(TDTO itemDTO) {
            var item = Mapper.Map<TModel>(itemDTO);
            await Repository.UpdateAsync(item);
        }
        public async Task DeleteAsync(int id) {
            await Repository.DeleteAsync(id);
        }
    }
}
