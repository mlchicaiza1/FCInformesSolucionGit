using AutoMapper;
using FCInformesSolucion.DAL;
using FCInformesSolucion.DAL.Entities;
using FCInformesSolucion.Models;
using FCInformesSolucion.Services;
using System;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using Unity;
using Unity.AspNet.Mvc;

namespace FCInformesSolucion
{
    
    public static class AutoMapperConfig
    {
        public static MapperConfiguration Configure() {

            var configuration = new MapperConfiguration(cfg =>
            {   
                cfg.CreateMap<DAL.Entities.Menu, MenuModel>();
                cfg.CreateMap<DAL.Entities.Menu, MenuViewModel>();

                cfg.CreateMap<MenuViewModel, MenuModel>();
                cfg.CreateMap<MenuViewModel, DAL.Entities.Menu>();                

                cfg.CreateMap<MenuModel, MenuViewModel>();
                cfg.CreateMap<MenuModel, DAL.Entities.Menu>();


                cfg.CreateMap<Province, ProvinceModel>();
                cfg.CreateMap<Province, ProvinceViewModel>();

                cfg.CreateMap<ProvinceViewModel, ProvinceModel>();
                cfg.CreateMap<ProvinceViewModel, Province>();

                cfg.CreateMap<ProvinceModel, ProvinceViewModel>();
                cfg.CreateMap<ProvinceModel, Province>();


                cfg.CreateMap<City, CityModel>();
                cfg.CreateMap<City, CityViewModel>();

                cfg.CreateMap<CityViewModel, CityModel>();
                cfg.CreateMap<CityViewModel, City>();

                cfg.CreateMap<CityModel, CityViewModel>();
                cfg.CreateMap<CityModel, City>();

                cfg.CreateMap<Bank, BankModel>();
                cfg.CreateMap<Bank, BankViewModel>();

                cfg.CreateMap<BankViewModel, BankModel>();
                cfg.CreateMap<BankViewModel, Bank>();

                cfg.CreateMap<BankModel, BankViewModel>();
                cfg.CreateMap<BankModel, Bank>();

                cfg.CreateMap<Request, RequestModel>();
                cfg.CreateMap<Request, RequestViewModel>();

                cfg.CreateMap<RequestViewModel, RequestModel>();
                cfg.CreateMap<RequestViewModel, Request>();

                cfg.CreateMap<RequestModel, RequestViewModel>();
                cfg.CreateMap<RequestModel, Request>();

                cfg.CreateMap<Agency, AgencyModel>();
                cfg.CreateMap<Agency, AgencyViewModel>();

                cfg.CreateMap<AgencyViewModel, AgencyModel>();
                cfg.CreateMap<AgencyViewModel, Agency>();

                cfg.CreateMap<AgencyModel, AgencyViewModel>();
                cfg.CreateMap<AgencyModel, Agency>();
            }); 

            return configuration;
        }
    }
}