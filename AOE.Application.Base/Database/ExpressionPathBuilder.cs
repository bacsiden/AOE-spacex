using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace AOE.Application.Base.Database
{
    public class ExpressionPathBuilder : ExpressionVisitor
    {
        private readonly StringBuilder _out = new StringBuilder();

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return _out.ToString();
        }

        /// <summary>
        /// Builds a path string from the expression.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <returns></returns>
        public static string ExpressionToPath(Expression node)
        {
            var expressionPathBuilder = new ExpressionPathBuilder();
            var node1 = node;
            expressionPathBuilder.Visit(node1);
            return expressionPathBuilder.ToString().Substring(1);
        }

        /// <summary>
        /// Visits the lambda.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="node">The node.</param>
        /// <returns></returns>
        protected override Expression VisitLambda<T>(Expression<T> node)
        {
            Visit(node.Body);
            return node;
        }

        /// <summary>
        /// Visits the member.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <returns></returns>
        protected override Expression VisitMember(MemberExpression node)
        {
            OutMember(node.Expression, node.Member);
            return node;
        }

        /// <summary>
        /// Visits the method call.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <returns></returns>
        protected override Expression VisitMethodCall(MethodCallExpression node)
        {
            if (node.Method.Name != "get_Item")
            {
                return node;
            }
            Visit(node.Object);

            var argumentValue = Expression.Lambda(node.Arguments[0]).Compile().DynamicInvoke().ToString();

            Out(".");
            Out(argumentValue);

            return node;
        }

        private void Out(string s)
        {
            _out.Append(s);
        }

        private void OutMember(Expression instance, MemberInfo member)
        {
            if (instance != null)
            {
                Visit(instance);
                Out("." + member.Name);
            }
            else
                Out(member.DeclaringType.Name + "." + member.Name);
        }
    }
}
