﻿using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CsvHelper.Configuration;

namespace CsvHelper.Tests
{
	[TestClass]
	public class CsvReaderReferenceMappingTests
	{
		[TestMethod]
		public void NestedReferencesClassMappingTest()
		{
			using( var stream = new MemoryStream() )
			using( var reader = new StreamReader( stream ) )
			using( var writer = new StreamWriter( stream ) )
			using( var csv = new CsvReader( reader ) )
			{
				csv.Configuration.ClassMapping<AMap>();

				writer.WriteLine( "AId,BId,CId,DId" );
				writer.WriteLine( "a1,b1,c1,d1" );
				writer.WriteLine( "a2,b2,c2,d2" );
				writer.WriteLine( "a3,b3,c3,d3" );
				writer.WriteLine( "a4,b4,c4,d4" );
				writer.Flush();
				stream.Position = 0;

				var list = csv.GetRecords<A>().ToList();

				Assert.IsNotNull( list );
				Assert.AreEqual( 4, list.Count );

				for( var i = 0; i < 4; i++ )
				{
					var rowId = i + 1;
					var row = list[i];
					Assert.AreEqual( "a" + rowId, row.Id );
					Assert.AreEqual( "b" + rowId, row.B.Id );
					Assert.AreEqual( "c" + rowId, row.B.C.Id );
					Assert.AreEqual( "d" + rowId, row.B.C.D.Id );
				}
			}
		}

		[TestMethod]
		[Ignore]
		public void NestedReferencesAttributeMappingTest()
		{
			// TODO: Remove this test when attribute mapping goes away.
			using( var stream = new MemoryStream() )
			using( var reader = new StreamReader( stream ) )
			using( var writer = new StreamWriter( stream ) )
			using( var csv = new CsvReader( reader ) )
			{
				writer.WriteLine( "AId,BId,CId,DId" );
				writer.WriteLine( "a1,b1,c1,d1" );
				writer.WriteLine( "a2,b2,c2,d2" );
				writer.WriteLine( "a3,b3,c3,d3" );
				writer.WriteLine( "a4,b4,c4,d4" );
				writer.Flush();
				stream.Position = 0;

				var list = csv.GetRecords<A>().ToList();

				Assert.IsNotNull( list );
				Assert.AreEqual( 4, list.Count );

				for( var i = 0; i < 4; i++ )
				{
					var rowId = i + 1;
					var row = list[i];
					Assert.AreEqual( "a" + rowId, row.Id );
					Assert.AreEqual( "b" + rowId, row.B.Id );
					Assert.AreEqual( "c" + rowId, row.B.C.Id );
					Assert.AreEqual( "d" + rowId, row.B.C.D.Id );
				}
			}
		}

		private class A
		{
			[CsvField( Name = "AId" )]
			public string Id { get; set; }

			[CsvField( ReferenceKey = "B" )]
			public B B { get; set; }
		}

		private class B
		{
			[CsvField( ReferenceKey = "B", Name = "BId" )]
			public string Id { get; set; }

			[CsvField( ReferenceKey = "C" )]
			public C C { get; set; }
		}

		private class C
		{
			[CsvField( ReferenceKey = "C", Name = "CId" )]
			public string Id { get; set; }

			[CsvField( ReferenceKey = "D" )]
			public D D { get; set; }
		}

		private class D
		{
			[CsvField( ReferenceKey = "D", Name = "DId" )]
			public string Id { get; set; }
		}

		private sealed class AMap : CsvClassMap<A>
		{
			public AMap()
			{
				Map( m => m.Id ).Name( "AId" );
				References<BMap>( m => m.B );
			}
		}

		private sealed class BMap : CsvClassMap<B>
		{
			public BMap()
			{
				Map( m => m.Id ).Name( "BId" );
				References<CMap>( m => m.C );
			}
		}

		private sealed class CMap : CsvClassMap<C>
		{
			public CMap()
			{
				Map( m => m.Id ).Name( "CId" );
				References<DMap>( m => m.D );
			}
		}

		private sealed class DMap : CsvClassMap<D>
		{
			public DMap()
			{
				Map( m => m.Id ).Name( "DId" );
			}
		}

	}
}