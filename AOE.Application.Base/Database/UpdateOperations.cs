using MongoDB.Driver;
using System;
using System.Linq.Expressions;

namespace AOE.Application.Base.Database
{
    public class UpdateOperations<T>
    {
        private readonly UpdateDefinitionBuilder<T> _builder;
        private UpdateDefinition<T> _updateDefinition;

        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateOperations{T}"/> class.
        /// </summary>
        public UpdateOperations()
        {
            _builder = Builders<T>.Update;
        }

        /// <summary>
        /// Sets the specified expression.
        /// </summary>
        /// <typeparam name="TMember">The type of the member.</typeparam>
        /// <param name="expression">The expression.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public UpdateOperations<T> Set<TMember>(Expression<Func<T, TMember>> expression, TMember value)
        {
            var path = ExpressionPathBuilder.ExpressionToPath(expression);

            _updateDefinition = _updateDefinition == null ? _builder.Set(path, value.GetBsonValue()) : _updateDefinition.Set(path, value.GetBsonValue());

            return this;
        }

        public UpdateDefinition<T> GetDefinition()
        {
            return _updateDefinition;
        }
    }
}
