using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using HexMaster.BuildingBlocks.EventBus;
using HexMaster.BuildingBlocks.EventBus.Abstractions;
using HexMaster.BuildingBlocks.EventBus.Configuration;
using HexMaster.BuildingBlocks.EventBus.EventBusServiceBus;
using HexMaster.Transactions.AspNetCore.Hubs;
using HexMaster.Transactions.AspNetCore.IntegrationEvents.Events;
using HexMaster.Transactions.AspNetCore.IntegrationEvents.Handlers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace HexMaster.Transactions.AspNetCore
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            var eventBusSection = Configuration.GetSection(EventBusConfiguration.SettingName);
            services.Configure<EventBusConfiguration>(eventBusSection);
            var eventBusSettings = eventBusSection.Get<EventBusConfiguration>();


            ConfigureEventBus(services, eventBusSettings);
            RegisterEventBus(services, eventBusSettings);

            services.AddSignalR();

            var containerBuilder = new ContainerBuilder();
            containerBuilder.Populate(services);

            var container = containerBuilder.Build();
            return new AutofacServiceProvider(container);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            ConfigureEventBus(app);
            app.UseHttpsRedirection();
            app.UseSignalR(routes =>
            {
                routes.MapHub<TransactionsHub>("/hubs/transactions");
            });
            app.UseMvc();
        }


        private void ConfigureEventBus(IServiceCollection services, EventBusConfiguration settings)
        {
                services.AddSingleton<IServiceBusPersisterConnection>(sp =>
                {
                    var logger = sp.GetRequiredService<ILogger<DefaultServiceBusPersisterConnection>>();
                    var serviceBusConnection = new ServiceBusConnectionStringBuilder(settings.EventBusConnection);
                    return new DefaultServiceBusPersisterConnection(serviceBusConnection, logger);
                });
        }


        private void RegisterEventBus(IServiceCollection services, EventBusConfiguration settings)
        {
            var subscriptionClientName = settings.SubscriptionClientName;
                services.AddSingleton<IEventBus, EventBusServiceBus>(sp =>
                {
                    var serviceBusPersisterConnection = sp.GetRequiredService<IServiceBusPersisterConnection>();
                    var iLifetimeScope = sp.GetRequiredService<ILifetimeScope>();
                    var logger = sp.GetRequiredService<ILogger<EventBusServiceBus>>();
                    var eventBusSubcriptionsManager = sp.GetRequiredService<IEventBusSubscriptionsManager>();

                    return new EventBusServiceBus(serviceBusPersisterConnection, logger,
                        eventBusSubcriptionsManager, subscriptionClientName, iLifetimeScope);
                });


            services.AddSingleton<IEventBusSubscriptionsManager, InMemoryEventBusSubscriptionsManager>();
            services.AddTransient<TransactionCreatedIntegrationEventHandler>();
            services.AddTransient<TransactionCreateFailedIntegrationEventHandler>();
            
        }

        protected virtual void ConfigureEventBus(IApplicationBuilder app)
        {
            var eventBus = app.ApplicationServices.GetRequiredService<IEventBus>();
            eventBus.Subscribe<TransactionCreatedIntegrationEvent, TransactionCreatedIntegrationEventHandler>();
            eventBus.Subscribe<TransactionCreateFailedIntegrationEvent, TransactionCreateFailedIntegrationEventHandler>();
        }
    }
}
