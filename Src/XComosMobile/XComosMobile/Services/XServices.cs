using System;
using System.Collections.Generic;
using System.Text;

namespace XComosMobile.Services
{
    public class XServices
    {

        Dictionary<Type, object> services;

        static XServices m_Instance = null;
        public static XServices Instance
        {
            get
            {
                if (m_Instance == null)
                    m_Instance = new XServices();
                return m_Instance;
            }
        }

        public XServices()
        {
            services = new Dictionary<Type, object>();
        }

        private void AddService(Type type, object provider)
        {
            if (type == null)
                throw new ArgumentNullException("type");
            if (provider == null)
                throw new ArgumentNullException("provider");

            if (services.ContainsKey(type))
                RemoveService(type);

            services.Add(type, provider);
        }

        private object GetService(Type type)
        {
            if (type == null)
                throw new ArgumentNullException("type");

            object service;
            if (services.TryGetValue(type, out service))
                return service;

            return null;
        }

        private void RemoveService(Type type)
        {
            if (type == null)
                throw new ArgumentNullException("type");

            services.Remove(type);
        }

        public void AddService<T>(T provider)
        {
            AddService(typeof(T), provider);
        }

        public void RemoveService<T>()
        {
            RemoveService(typeof(T));
        }

        public T GetService<T>() where T : class
        {
            var service = GetService(typeof(T));

            if (service == null)
                return null;

            return (T)service;
        }
    }
}
