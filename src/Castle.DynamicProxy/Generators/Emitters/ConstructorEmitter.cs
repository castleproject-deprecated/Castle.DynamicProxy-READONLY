// Copyright 2004-2010 Castle Project - http://www.castleproject.org/
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//     http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

namespace Castle.DynamicProxy.Generators.Emitters
{
	using System;
	using System.Reflection;
	using System.Reflection.Emit;
	using Castle.DynamicProxy.Generators.Emitters.CodeBuilders;
	using Castle.DynamicProxy.Generators.Emitters.SimpleAST;

	public class ConstructorEmitter : IMemberEmitter
	{
		private readonly ConstructorBuilder builder;
		private readonly AbstractTypeEmitter maintype;

		private ConstructorCodeBuilder constructorCodeBuilder;

		internal ConstructorEmitter(AbstractTypeEmitter maintype, params ArgumentReference[] arguments)
		{
			this.maintype = maintype;

			Type[] args = ArgumentsUtil.InitializeAndConvert(arguments);

			builder = maintype.TypeBuilder.DefineConstructor(
				MethodAttributes.Public, CallingConventions.Standard, args);
		}

		protected internal ConstructorEmitter(AbstractTypeEmitter maintype, ConstructorBuilder builder)
		{
			this.maintype = maintype;
			this.builder = builder;
		}

		public virtual ConstructorCodeBuilder CodeBuilder
		{
			get
			{
				if (constructorCodeBuilder == null)
				{
					constructorCodeBuilder = new ConstructorCodeBuilder(
						maintype.BaseType, builder.GetILGenerator());
				}
				return constructorCodeBuilder;
			}
		}

		public ConstructorBuilder ConstructorBuilder
		{
			get { return builder; }
		}

		public MemberInfo Member
		{
			get { return builder; }
		}

		public Type ReturnType
		{
			get { return typeof (void); }
		}

		public virtual void Generate()
		{
			CodeBuilder.Generate(this, builder.GetILGenerator());
		}

		public virtual void EnsureValidCodeBlock()
		{
			if (CodeBuilder.IsEmpty)
			{
				CodeBuilder.InvokeBaseConstructor();
				CodeBuilder.AddStatement(new ReturnStatement());
			}
		}
	}
}