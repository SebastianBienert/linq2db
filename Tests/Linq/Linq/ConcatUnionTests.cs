﻿using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using LinqToDB;
using LinqToDB.Mapping;

using NUnit.Framework;

namespace Tests.Linq
{
	using Model;


	[TestFixture]
	public class ConcatUnionTests : TestBase
	{
		[Test]
		public void Concat1([DataSources] string context)
		{
			using (var db = GetDataContext(context))
				AreEqual(
					(from p in Parent where p.ParentID == 1 select p).Concat(
					(from p in Parent where p.ParentID == 2 select p))
					,
					(from p in db.Parent where p.ParentID == 1 select p).Concat(
					(from p in db.Parent where p.ParentID == 2 select p)));
		}

		[Test]
		public async Task Concat1Async([DataSources] string context)
		{
			using (var db = GetDataContext(context))
				AreEqual(
					(from p in Parent where p.ParentID == 1 select p).Concat(
					(from p in Parent where p.ParentID == 2 select p))
					,
					await
					(from p in db.Parent where p.ParentID == 1 select p).Concat(
					(from p in db.Parent where p.ParentID == 2 select p))
					.ToListAsync());
		}

		[Test]
		public void Concat11([DataSources] string context)
		{
			using (var db = GetDataContext(context))
				AreEqual(
					(from ch in    Child where ch.ParentID == 1 select ch.Parent).Concat(
					(from ch in    Child where ch.ParentID == 2 select ch.Parent)),
					(from ch in db.Child where ch.ParentID == 1 select ch.Parent).Concat(
					(from ch in db.Child where ch.ParentID == 2 select ch.Parent)));
		}

		[Test]
		public void Concat12([DataSources] string context)
		{
			using (var db = GetDataContext(context))
				AreEqual(
					(from p  in    Parent where p.ParentID  == 1 select p).Concat(
					(from ch in    Child  where ch.ParentID == 2 select ch.Parent)),
					(from p  in db.Parent where p.ParentID  == 1 select p).Concat(
					(from ch in db.Child  where ch.ParentID == 2 select ch.Parent)));
		}

		[Test]
		public void Concat2([DataSources] string context)
		{
			using (var db = GetDataContext(context))
				AreEqual(
					(from p in Parent where p.ParentID == 1 select p).Concat(
					(from p in Parent where p.ParentID == 2 select p)).Concat(
					(from p in Parent where p.ParentID == 4 select p))
					,
					(from p in db.Parent where p.ParentID == 1 select p).Concat(
					(from p in db.Parent where p.ParentID == 2 select p)).Concat(
					(from p in db.Parent where p.ParentID == 4 select p)));
		}

		[Test]
		public void Concat3([DataSources] string context)
		{
			using (var db = GetDataContext(context))
				AreEqual(
					(from p in Parent where p.ParentID == 1 select p).Concat(
					(from p in Parent where p.ParentID == 2 select p).Concat(
					(from p in Parent where p.ParentID == 4 select p)))
					,
					(from p in db.Parent where p.ParentID == 1 select p).Concat(
					(from p in db.Parent where p.ParentID == 2 select p).Concat(
					(from p in db.Parent where p.ParentID == 4 select p))));
		}

		[Test]
		public void Concat4([DataSources] string context)
		{
			using (var db = GetDataContext(context))
				AreEqual(
					(from c in    Child where c.ParentID == 1 select c).Concat(
					(from c in    Child where c.ParentID == 3 select new Child { ParentID = c.ParentID, ChildID = c.ChildID + 1000 }).
					Where(c => c.ChildID != 1032))
					,
					(from c in db.Child where c.ParentID == 1 select c).Concat(
					(from c in db.Child where c.ParentID == 3 select new Child { ParentID = c.ParentID, ChildID = c.ChildID + 1000 })).
					Where(c => c.ChildID != 1032));
		}

		[Test]
		public void Concat401([DataSources] string context)
		{
			using (var db = GetDataContext(context))
				AreEqual(
					(from c in    Child where c.ParentID == 1 select c).Concat(
					(from c in    Child where c.ParentID == 3 select new Child { ChildID = c.ChildID + 1000, ParentID = c.ParentID }).
					Where(c => c.ChildID != 1032))
					,
					(from c in db.Child where c.ParentID == 1 select c).Concat(
					(from c in db.Child where c.ParentID == 3 select new Child { ChildID = c.ChildID + 1000, ParentID = c.ParentID })).
					Where(c => c.ChildID != 1032));
		}

		[Test]
		public void Concat5([DataSources(ProviderName.DB2, TestProvName.AllInformix)] string context)
		{
			using (var db = GetDataContext(context))
				AreEqual(
					(from c in    Child where c.ParentID == 1 select c).Concat(
					(from c in    Child where c.ParentID == 3 select new Child { ChildID = c.ChildID + 1000 }).
					Where(c => c.ChildID != 1032))
					,
					(from c in db.Child where c.ParentID == 1 select c).Concat(
					(from c in db.Child where c.ParentID == 3 select new Child { ChildID = c.ChildID + 1000 })).
					Where(c => c.ChildID != 1032));
		}

		[Test]
		public void Concat501([DataSources(ProviderName.DB2, TestProvName.AllInformix)] string context)
		{
			using (var db = GetDataContext(context))
				AreEqual(
					(from c in    Child where c.ParentID == 1 select new Child { ParentID = c.ParentID }).Concat(
					(from c in    Child where c.ParentID == 3 select new Child { ChildID  = c.ChildID + 1000 }).
					Where(c => c.ParentID == 1))
					,
					(from c in db.Child where c.ParentID == 1 select new Child { ParentID = c.ParentID }).Concat(
					(from c in db.Child where c.ParentID == 3 select new Child { ChildID  = c.ChildID + 1000 })).
					Where(c => c.ParentID == 1));
		}

		[Test]
		public void Concat502([DataSources(ProviderName.DB2, TestProvName.AllInformix)] string context)
		{
			using (var db = GetDataContext(context))
				AreEqual(
					(from c in    Child where c.ParentID == 1 select c.Parent).Concat(
					(from c in    Child where c.ParentID == 3 select c.Parent).
					Where(p => p.Value1!.Value != 2))
					,
					(from c in db.Child where c.ParentID == 1 select c.Parent).Concat(
					(from c in db.Child where c.ParentID == 3 select c.Parent)).
					Where(p => p.Value1!.Value != 2));
		}

		[Test]
		public void Concat6([DataSources(ProviderName.SqlCe)] string context)
		{
			using (var db = GetDataContext(context))
				AreEqual(
					   Child.Where(c => c.GrandChildren.Count == 2).Concat(   Child.Where(c => c.GrandChildren.Count() == 3)),
					db.Child.Where(c => c.GrandChildren.Count == 2).Concat(db.Child.Where(c => c.GrandChildren.Count() == 3)));
		}

		[Test]
		public void Concat7([NorthwindDataContext] string context)
		{
			using (var db = new NorthwindDB(context))
			{
				var dd = GetNorthwindAsList(context);
				AreEqual(
					dd.Customer.Where(c => c.Orders.Count <= 1).Concat(dd.Customer.Where(c => c.Orders.Count > 1)),
					db.Customer.Where(c => c.Orders.Count <= 1).Concat(db.Customer.Where(c => c.Orders.Count > 1)));
			}
		}

		[Test]
		public void Concat81([DataSources] string context)
		{
			using (var db = GetDataContext(context))
				AreEqual(
					   Parent.Select(c => new { ID1 = c.ParentID, ID2 = c.ParentID, }).Concat(
					   Child. Select(c => new { ID1 = c.ParentID, ID2 = c.ChildID,  })),
					db.Parent.Select(c => new { ID1 = c.ParentID, ID2 = c.ParentID, }).Concat(
					db.Child. Select(c => new { ID1 = c.ParentID, ID2 = c.ChildID,  })));
		}

		[Test]
		public void Concat82([DataSources] string context)
		{
			using (var db = GetDataContext(context))
				AreEqual(
					   Child. Select(c => new { ID1 = c.ParentID, ID2 = c.ChildID,  }).Concat(
					   Parent.Select(c => new { ID1 = c.ParentID, ID2 = c.ParentID, })),
					db.Child. Select(c => new { ID1 = c.ParentID, ID2 = c.ChildID,  }).Concat(
					db.Parent.Select(c => new { ID1 = c.ParentID, ID2 = c.ParentID, })));
		}

		[Test]
		public void Concat83([DataSources] string context)
		{
			using (var db = GetDataContext(context))
				AreEqual(
					   Parent.Select(c => new { ID1 = c.ParentID, ID2 = c.ParentID, ID3 = c.Value1 ?? 0,  }).Concat(
					   Child. Select(c => new { ID1 = c.ParentID, ID2 = c.ChildID,  ID3 = c.ParentID + 1, })),
					db.Parent.Select(c => new { ID1 = c.ParentID, ID2 = c.ParentID, ID3 = c.Value1 ?? 0,  }).Concat(
					db.Child. Select(c => new { ID1 = c.ParentID, ID2 = c.ChildID,  ID3 = c.ParentID + 1, })));
		}

		[Test]
		public void Concat84([DataSources] string context)
		{
			using (var db = GetDataContext(context))
				AreEqual(
					   Child. Select(c => new { ID1 = c.ParentID, ID2 = c.ChildID,  ID3 = c.ParentID + 1, }).Concat(
					   Parent.Select(c => new { ID1 = c.ParentID, ID2 = c.ParentID, ID3 = c.Value1 ?? 0,  })),
					db.Child. Select(c => new { ID1 = c.ParentID, ID2 = c.ChildID,  ID3 = c.ParentID + 1, }).Concat(
					db.Parent.Select(c => new { ID1 = c.ParentID, ID2 = c.ParentID, ID3 = c.Value1 ?? 0,  })));
		}

		[Test]
		public void Concat85([DataSources] string context)
		{
			using (var db = GetDataContext(context))
				AreEqual(
					   Parent.Select(c => new { ID1 = c.ParentID, ID2 = c.Value1 ?? 0,  ID3 = c.ParentID, }).Concat(
					   Child. Select(c => new { ID1 = c.ParentID, ID2 = c.ParentID + 1, ID3 = c.ChildID,  })),
					db.Parent.Select(c => new { ID1 = c.ParentID, ID2 = c.Value1 ?? 0,  ID3 = c.ParentID, }).Concat(
					db.Child. Select(c => new { ID1 = c.ParentID, ID2 = c.ParentID + 1, ID3 = c.ChildID,  })));
		}

		[Test]
		public void Concat851([DataSources] string context)
		{
			using (var db = GetDataContext(context))
				AreEqual(
					   Parent.Select(c => new { ID1 = c.ParentID, ID2 = c.ParentID,     ID3 = c.ParentID, }).Concat(
					   Child. Select(c => new { ID1 = c.ParentID, ID2 = c.ParentID + 1, ID3 = c.ChildID,  })),
					db.Parent.Select(c => new { ID1 = c.ParentID, ID2 = c.ParentID,     ID3 = c.ParentID, }).Concat(
					db.Child. Select(c => new { ID1 = c.ParentID, ID2 = c.ParentID + 1, ID3 = c.ChildID,  })));
		}

		[Test]
		public void Concat86([DataSources] string context)
		{
			using (var db = GetDataContext(context))
				AreEqual(
					   Child. Select(c => new { ID1 = c.ParentID, ID2 = c.ParentID + 1, ID3 = c.ChildID,  }).Concat(
					   Parent.Select(c => new { ID1 = c.ParentID, ID2 = c.Value1 ?? 0,  ID3 = c.ParentID, })),
					db.Child. Select(c => new { ID1 = c.ParentID, ID2 = c.ParentID + 1, ID3 = c.ChildID,  }).Concat(
					db.Parent.Select(c => new { ID1 = c.ParentID, ID2 = c.Value1 ?? 0,  ID3 = c.ParentID, })));
		}

		[Test]
		public void Concat87([DataSources(TestProvName.AllInformix)] string context)
		{
			using (var db = GetDataContext(context))
				AreEqual(
					   Child. Select(c => new Parent { ParentID = c.ParentID }).Concat(
					   Parent.Select(c => new Parent { Value1   = c.Value1   })),
					db.Child. Select(c => new Parent { ParentID = c.ParentID }).Concat(
					db.Parent.Select(c => new Parent { Value1   = c.Value1   })));
		}

		[Test]
		public void Concat871([DataSources(TestProvName.AllInformix)] string context)
		{
			using (var db = GetDataContext(context))
				AreEqual(
					   Parent.Select(c => new Parent { Value1   = c.Value1   }).Concat(
					   Child. Select(c => new Parent { ParentID = c.ParentID })),
					db.Parent.Select(c => new Parent { Value1   = c.Value1   }).Concat(
					db.Child. Select(c => new Parent { ParentID = c.ParentID })));
		}

		[Test]
		public void Concat88([DataSources] string context)
		{
			using (var db = GetDataContext(context))
				AreEqual(
					   Child. Select(c => new Parent { Value1   = c.ChildID,  ParentID = c.ParentID }).Concat(
					   Parent.Select(c => new Parent { ParentID = c.ParentID, Value1   = c.Value1   })),
					db.Child. Select(c => new Parent { Value1   = c.ChildID,  ParentID = c.ParentID }).Concat(
					db.Parent.Select(c => new Parent { ParentID = c.ParentID, Value1   = c.Value1   })));
		}

		[Test]
		public void Concat89([DataSources(TestProvName.AllInformix)] string context)
		{
			using (var db = GetDataContext(context))
				AreEqual(
					   Child. Select(c => new Parent { Value1 = c.ParentID, ParentID = c.ParentID }).Concat(
					   Parent.Select(c => new Parent {                      ParentID = c.ParentID })),
					db.Child. Select(c => new Parent { Value1 = c.ParentID, ParentID = c.ParentID }).Concat(
					db.Parent.Select(c => new Parent {                      ParentID = c.ParentID })));
		}

		[Test]
		public void Concat891([DataSources(TestProvName.AllInformix)] string context)
		{
			using (var db = GetDataContext(context))
				AreEqual(
					   Child. Select(c => new Parent { Value1 = c.ParentID, ParentID = c.ParentID }).Union(
					   Parent.Select(c => new Parent {                      ParentID = c.ParentID })).Concat(
					   Child. Select(c => new Parent { Value1 = c.ParentID, ParentID = c.ParentID })/*.Union(
					   Parent.Select(c => new Parent {                      ParentID = c.ParentID }))*/),
					db.Child. Select(c => new Parent { Value1 = c.ParentID, ParentID = c.ParentID }).Union(
					db.Parent.Select(c => new Parent {                      ParentID = c.ParentID })).Concat(
					db.Child. Select(c => new Parent { Value1 = c.ParentID, ParentID = c.ParentID })/*.Union(
					db.Parent.Select(c => new Parent {                      ParentID = c.ParentID }))*/),
					sort: x => x.OrderBy(_ => _.ParentID).ThenBy(_ => _.Value1));
		}

		[Test]
		public void Concat892([DataSources(TestProvName.AllInformix)] string context)
		{
			using var db = GetDataContext(context);

			var query = db.Child.Select(c => new Parent {Value1 = c.ParentID, ParentID = c.ParentID})
				.Union(db.Parent.Select(c => new Parent {                     ParentID = c.ParentID}))
				.Concat(db.Child.Select(c => new Parent {Value1 = c.ParentID, ParentID = c.ParentID}));

			AssertQuery(query);
		}

		[Test]
		public void Union1([DataSources] string context)
		{
			using (var db = GetDataContext(context))
				AreEqual(
					(from g  in    GrandChild join ch in    Child  on g.ChildID   equals ch.ChildID select ch).Union(
					(from ch in    Child      join p  in    Parent on ch.ParentID equals p.ParentID select ch))
					,
					(from g  in db.GrandChild join ch in db.Child  on g.ChildID   equals ch.ChildID select ch).Union(
					(from ch in db.Child      join p  in db.Parent on ch.ParentID equals p.ParentID select ch)));
		}

		[Test]
		public void Union2([DataSources] string context)
		{
			using (var db = GetDataContext(context))
				AreEqual(
					from r  in
						(from g  in GrandChild join ch in Child  on g.ChildID   equals ch.ChildID select ch.ChildID).Union(
						(from ch in Child      join p  in Parent on ch.ParentID equals p.ParentID select ch.ChildID))
					join child in Child on r equals child.ChildID
					select child
					,
					from r in
						(from g  in db.GrandChild join ch in db.Child  on g.ChildID   equals ch.ChildID select ch.ChildID).Union(
						(from ch in db.Child      join p  in db.Parent on ch.ParentID equals p.ParentID select ch.ChildID))
					join child in db.Child on r equals child.ChildID
					select child);
		}

		[Test]
		public void Union3([DataSources] string context)
		{
			using (var db = GetDataContext(context))
				AreEqual(
					(from p  in    Parent select new { id = p.ParentID,  val = true }).Union(
					(from ch in    Child  select new { id = ch.ParentID, val = false }))
					,
					(from p  in db.Parent select new { id = p.ParentID,  val = true }).Union(
					(from ch in db.Child  select new { id = ch.ParentID, val = false })));
		}

		[Test]
		public void Union4([DataSources] string context)
		{
			using (var db = GetDataContext(context))
				AreEqual(
					(from p  in    Parent select new { id = p.ParentID,  val = true }).Union(
					(from ch in    Child  select new { id = ch.ParentID, val = false }))
					.Select(p => new { p.id, p.val })
					,
					(from p  in db.Parent select new { id = p.ParentID,  val = true }).Union(
					(from ch in db.Child  select new { id = ch.ParentID, val = false }))
					.Select(p => new { p.id, p.val }));
		}

		[Test]
		public void Union41([DataSources] string context)
		{
			using (var db = GetDataContext(context))
				AreEqual(
					(from p  in    Parent select new { id = p.ParentID,  val = true }).Union(
					(from ch in    Child  select new { id = ch.ParentID, val = false }))
					.Select(p => p)
					,
					(from p  in db.Parent select new { id = p.ParentID,  val = true }).Union(
					(from ch in db.Child  select new { id = ch.ParentID, val = false }))
					.Select(p => p));
		}

		[Test]
		public void Union42([DataSources] string context)
		{
			using (var db = GetDataContext(context))
				AreEqual(
					(from p  in    Parent select new { id = p. ParentID, val = true  }).Union(
					(from ch in    Child  select new { id = ch.ParentID, val = false }))
					.Select(p => p.val),
					(from p  in db.Parent select new { id = p. ParentID, val = true  }).Union(
					(from ch in db.Child  select new { id = ch.ParentID, val = false }))
					.Select(p => p.val));
		}

		[Test]
		public void Union421([DataSources] string context)
		{
			using (var db = GetDataContext(context))
				AreEqual(
					(from p  in    Parent select new { id = p. ParentID, val = true  }).Union(
					(from p  in    Parent select new { id = p. ParentID, val = false }).Union(
					(from ch in    Child  select new { id = ch.ParentID, val = false })))
					.Select(p => p.val),
					(from p  in db.Parent select new { id = p. ParentID, val = true  }).Union(
					(from p  in db.Parent select new { id = p. ParentID, val = false }).Union(
					(from ch in db.Child  select new { id = ch.ParentID, val = false })))
					.Select(p => p.val));
		}

		[Test]
		public void Union5([DataSources(TestProvName.AllInformix)] string context)
		{
			using (var db = GetDataContext(context))
				AreEqual(
					(from p1 in    Parent select p1).Union(
					(from p2 in    Parent select new Parent { ParentID = p2.ParentID }))
					.Select(p => new Parent { ParentID = p.ParentID, Value1 = p.Value1 })
					,
					(from p1 in db.Parent select p1).Union(
					(from p2 in db.Parent select new Parent { ParentID = p2.ParentID }))
					.Select(p => new Parent { ParentID = p.ParentID, Value1 = p.Value1 }));
		}

		[Test]
		public void Union51([DataSources(TestProvName.AllInformix)] string context)
		{
			using (var db = GetDataContext(context))
				AreEqual(
					(from p1  in   Parent select p1).Union(
					(from p2 in    Parent select new Parent { ParentID = p2.ParentID }))
					,
					(from p1 in db.Parent select p1).Union(
					(from p2 in db.Parent select new Parent { ParentID = p2.ParentID })));
		}

		[Test]
		public void Union52([DataSources(TestProvName.AllAccess, TestProvName.AllInformix)] string context)
		{
			using (var db = GetDataContext(context))
				AreEqual(
					(from p1 in    Parent select new Parent { ParentID = p1.ParentID }).Union(
					(from p2 in    Parent select p2))
					,
					(from p1 in db.Parent select new Parent { ParentID = p1.ParentID }).Union(
					(from p2 in db.Parent select p2)));
		}

		[Test]
		public void Union521([DataSources(TestProvName.AllAccess, TestProvName.AllInformix)] string context)
		{
			using (var db = GetDataContext(context))
				AreEqual(
					(from p1 in    Parent select new Parent { ParentID = p1.ParentID }).Union(
					(from p2 in    Parent select p2))
					.Select(p => p.Value1)
					,
					(from p1 in db.Parent select new Parent { ParentID = p1.ParentID }).Union(
					(from p2 in db.Parent select p2))
					.Select(p => p.Value1));
		}

		[Test]
		public void Union522([DataSources(TestProvName.AllAccess, TestProvName.AllInformix)] string context)
		{
			using (var db = GetDataContext(context))
				AreEqual(
					(from p1 in    Parent select new Parent { Value1 = p1.Value1 }).Union(
					(from p2 in    Parent select p2))
					,
					(from p1 in db.Parent select new Parent { Value1 = p1.Value1 }).Union(
					(from p2 in db.Parent select p2)));
		}

		[Test]
		public void Union523([DataSources(TestProvName.AllAccess, TestProvName.AllInformix)] string context)
		{
			using (var db = GetDataContext(context))
				AreEqual(
					(from p1 in    Parent select new Parent { ParentID = p1.ParentID }).Union(
					(from p2 in    Parent select p2)),
					(from p1 in db.Parent select new Parent { ParentID = p1.ParentID }).Union(
					(from p2 in db.Parent select p2)));
		}

		[Test]
		public void Union53([DataSources(TestProvName.AllAccess, TestProvName.AllInformix)] string context)
		{
			using (var db = GetDataContext(context))
				AreEqual(
					(from p1 in    Parent select new Parent { ParentID = p1.ParentID }).Union(
					(from p2 in    Parent select new Parent { Value1   = p2.Value1   }))
					,
					(from p1 in db.Parent select new Parent { ParentID = p1.ParentID }).Union(
					(from p2 in db.Parent select new Parent { Value1   = p2.Value1   })));
		}

		//[Test]
		public void Union54([DataSources] string context)
		{
			using (var db = GetDataContext(context))
				AreEqual(
					(from p1 in    Parent select new { ParentID = p1.ParentID,    p = p1,            ch = (Child?)null }).Union(
					(from p2 in    Parent select new { ParentID = p2.Value1 ?? 0, p = (Parent?)null, ch = p2.Children.First() })),
					(from p1 in db.Parent select new { ParentID = p1.ParentID,    p = p1,            ch = (Child?)null }).Union(
					(from p2 in db.Parent select new { ParentID = p2.Value1 ?? 0, p = (Parent?)null, ch = p2.Children.First() })));
		}

		//[Test]
		public void Union541([DataSources] string context)
		{
			using (var db = GetDataContext(context))
				AreEqual(
					(from p1 in    Parent select new { ParentID = p1.ParentID,    p = p1,            ch = (Child?)null }).Union(
					(from p2 in    Parent select new { ParentID = p2.Value1 ?? 0, p = (Parent?)null, ch = p2.Children.First() }))
					.Select(p => new { p.ParentID, p.p, p.ch })
					,
					(from p1 in db.Parent select new { ParentID = p1.ParentID,    p = p1,            ch = (Child?)null }).Union(
					(from p2 in db.Parent select new { ParentID = p2.Value1 ?? 0, p = (Parent?)null, ch = p2.Children.First() }))
					.Select(p => new { p.ParentID, p.p, p.ch }));
		}

		[Test]
		public void ObjectUnion1([DataSources] string context)
		{
			using (var db = GetDataContext(context))
				AreEqual(
					(from p1 in    Parent where p1.ParentID >  3 select p1).Union(
					(from p2 in    Parent where p2.ParentID <= 3 select p2)),
					(from p1 in db.Parent where p1.ParentID >  3 select p1).Union(
					(from p2 in db.Parent where p2.ParentID <= 3 select p2)));
		}

		//////[Test]
		public void ObjectUnion2([DataSources] string context)
		{
			using (var db = GetDataContext(context))
				AreEqual(
					(from p1 in    Parent where p1.ParentID >  3 select p1).Union(
					(from p2 in    Parent where p2.ParentID <= 3 select (Parent?)null)),
					(from p1 in db.Parent where p1.ParentID >  3 select p1).Union(
					(from p2 in db.Parent where p2.ParentID <= 3 select (Parent?)null)));
		}

		[Test]
		public void ObjectUnion3([DataSources] string context)
		{
			using (var db = GetDataContext(context))
				AreEqual(
					(from p1 in    Parent where p1.ParentID >  3 select new { p = p1 }).Union(
					(from p2 in    Parent where p2.ParentID <= 3 select new { p = p2 })),
					(from p1 in db.Parent where p1.ParentID >  3 select new { p = p1 }).Union(
					(from p2 in db.Parent where p2.ParentID <= 3 select new { p = p2 })));
		}

		//////[Test]
		public void ObjectUnion4([DataSources] string context)
		{
			using (var db = GetDataContext(context))
				AreEqual(
					(from p1 in    Parent where p1.ParentID >  3 select new { p = new { p = p1, p1.ParentID } }).Union(
					(from p2 in    Parent where p2.ParentID <= 3 select new { p = new { p = p2, p2.ParentID } })),
					(from p1 in db.Parent where p1.ParentID >  3 select new { p = new { p = p1, p1.ParentID } }).Union(
					(from p2 in db.Parent where p2.ParentID <= 3 select new { p = new { p = p2, p2.ParentID } })));
		}

		//////[Test]
		public void ObjectUnion5([DataSources] string context)
		{
			using (var db = GetDataContext(context))
				AreEqual(
					(from p1 in    Parent where p1.ParentID >  3 select new { p = new { p = p1, ParentID = p1.ParentID + 1 } }).Union(
					(from p2 in    Parent where p2.ParentID <= 3 select new { p = new { p = p2, ParentID = p2.ParentID + 1 } })),
					(from p1 in db.Parent where p1.ParentID >  3 select new { p = new { p = p1, ParentID = p1.ParentID + 1 } }).Union(
					(from p2 in db.Parent where p2.ParentID <= 3 select new { p = new { p = p2, ParentID = p2.ParentID + 1 } })));
		}

		[Test]
		public void ObjectUnion([NorthwindDataContext] string context)
		{
			using (var db = new NorthwindDB(context))
			{
				var q1 =
					from p in db.Product
					join c in db.Category on p.CategoryID equals c.CategoryID into g
					from c in g.DefaultIfEmpty()
					select new
					{
						p,
						c.CategoryName,
						p.ProductName
					};

				var q2 =
					from p in db.Product
					join c in db.Category on p.CategoryID equals c.CategoryID into g
					from c in g.DefaultIfEmpty()
					select new
					{
						p,
						c.CategoryName,
						p.ProductName
					};

				var q = q1.Union(q2).Take(5);

				foreach (var item in q)
				{
					TestContext.WriteLine(item);
				}
			}
		}

		public class TestEntity1 { public int Id; public string? Field1; }
		public class TestEntity2 { public int Id; public string? Field1; }

		[Test]
		public void Concat90()
		{
			using(var context = new TestDataConnection())
			{
				var join1 =
					from t1 in context.GetTable<TestEntity1>()
					join t2 in context.GetTable<TestEntity2>()
						on t1.Id equals t2.Id
					into tmp
					from t2 in tmp.DefaultIfEmpty()
					select new { t1, t2 };

				var join1Sql = join1.ToString();
				Assert.IsNotNull(join1Sql);

				var join2 =
					from t2 in context.GetTable<TestEntity2>()
					join t1 in context.GetTable<TestEntity1>()
						on t2.Id equals t1.Id
					into tmp
					from t1 in tmp.DefaultIfEmpty()
					where t1 == null
					select new { t1, t2 };

				var join2Sql = join2.ToString();
				Assert.IsNotNull(join2Sql);

				var fullJoin = join1.Concat(join2);

				var fullJoinSql = fullJoin.ToString(); // BLToolkit.Data.Linq.LinqException : Types in Concat are constructed incompatibly.
				Assert.IsNotNull(fullJoinSql);
			}
		}

		[ActiveIssue("Associations with Concat/Union or other Set operations are not supported.")]
		[Test]
		public void AssociationUnion1([DataSources] string context)
		{
			using (var db = GetDataContext(context))
				AreEqual(
					from c in    Child.Union(Child)
					let p = c.Parent
					select p.ParentID,
					from c in db.Child.Union(db.Child)
					let p = c.Parent
					select p.ParentID);
		}

		[ActiveIssue("Associations with Concat/Union or other Set operations are not supported.")]
		[Test]
		public void AssociationUnion2([DataSources] string context)
		{
			using (var db = GetDataContext(context))
				AreEqual(
					from c in    Child.Union(Child)
					select c.Parent!.ParentID,
					from c in db.Child.Union(db.Child)
					select c.Parent!.ParentID);
		}

		[ActiveIssue("Associations with Concat/Union or other Set operations are not supported.")]
		[Test]
		public void AssociationConcat2([DataSources] string context)
		{
			using (var db = GetDataContext(context))
				AreEqual(
					from c in    Child.Concat(Child)
					select c.Parent!.ParentID,
					from c in db.Child.Concat(db.Child)
					select c.Parent!.ParentID);
		}

		[Test]
		public void ConcatToString([DataSources] string context)
		{
			string pattern = "1";

			using (var db = GetDataContext(context))
				AreEqual(
					(from p in Person where p.FirstName.Contains(pattern) select p.FirstName).Concat(
					(from p in Person where p.ID.ToString().Contains(pattern) select p.FirstName)).Take(10)
					,
					(from p in db.Person where Sql.Like(p.FirstName, "1") select p.FirstName).Concat(
					(from p in db.Person where p.ID.ToString().Contains(pattern) select p.FirstName)).Take(10));
		}

		[Test]
		public void ConcatWithUnion([DataSources] string context)
		{
			using (var db = GetDataContext(context))
				AreEqual(
					Parent.Select(c => new Parent {ParentID = c.ParentID}). Union(
					Parent.Select(c => new Parent {ParentID = c.ParentID})).Concat(
					Parent.Select(c => new Parent {ParentID = c.ParentID}). Union(
					Parent.Select(c => new Parent {ParentID = c.ParentID})
						)
					),
					db.Parent.Select(c => new Parent {ParentID = c.ParentID}). Union(
					db.Parent.Select(c => new Parent {ParentID = c.ParentID})).Concat(
					db.Parent.Select(c => new Parent {ParentID = c.ParentID}). Union(
					db.Parent.Select(c => new Parent {ParentID = c.ParentID})
						)
					)
				);
		}

		[Test]
		public void UnionWithObjects([DataSources] string context)
		{
			using (var db = GetDataContext(context))
			{
				var q1 =
					from p in db.Parent
					from p2 in db.Parent
					join c in db.Child on p.ParentID equals c.ParentID
					select new
					{
						P1 = p,
						P2 = p2,
						C = c
					};

				var q2 =
					from p in db.Parent
					from p2 in db.Parent
					join c in db.Child on p2.ParentID equals c.ParentID
					select new
					{
						P1 = p,
						P2 = p2,
						C = c
					};

				var q = q1.Union(q2);

				var qe1 =
					from p in Parent
					from p2 in Parent
					join c in Child on p.ParentID equals c.ParentID
					select new
					{
						P1 = p,
						P2 = p2,
						C = c
					};

				var qe2 =
					from p in Parent
					from p2 in Parent
					join c in Child on p2.ParentID equals c.ParentID
					select new
					{
						P1 = p,
						P2 = p2,
						C = c
					};

				var qe = qe1.Union(qe2);

				AreEqual(qe, q);
			}
		}

		[Test]
		public void UnionGroupByTest1([DataSources] string context)
		{
			using (var db = GetDataContext(context))
			{
				var actual =
					db.Types
						.GroupBy(_ => new { month = _.DateTimeValue.Month, year = _.DateTimeValue.Year })
						.Select(_ => _.Key)
						.Select(_ => new { _.month, _.year, @int = 1 })
					.Union(
						db.Types.Select(_ => new { month = (int)_.SmallIntValue, year = (int)_.SmallIntValue, @int = 3 }))
					.Union(
						db.Types.Select(_ => new { month = _.DateTimeValue.Year, year = _.DateTimeValue.Year, @int = 2 }))
//					.AsEnumerable()
//					.OrderBy(_ => _.month)
//					.ThenBy (_ => _.year)
//					.ThenBy (_ => _.@int)
					.ToList();

				var expected =
					GetTypes(context)
						.GroupBy(_ => new { month = _.DateTimeValue.Month, year = _.DateTimeValue.Year })
						.Select(_ => _.Key)
						.Select(_ => new { _.month, _.year, @int = 1 })
					.Union(
						GetTypes(context).Select(_ => new { month = (int)_.SmallIntValue, year = (int)_.SmallIntValue, @int = 3 }))
					.Union(
						GetTypes(context).Select(_ => new { month = _.DateTimeValue.Year, year = _.DateTimeValue.Year, @int = 2 }))
//					.AsEnumerable()
//					.OrderBy(_ => _.month)
//					.ThenBy (_ => _.year)
//					.ThenBy (_ => _.@int)
					.ToList();

				AreEqual(expected, actual);
			}
		}

		[Test]
		public void UnionGroupByTest2([DataSources] string context)
		{
			using (var db = GetDataContext(context))
			{
				var actual =
					db.Types.Select(_ => new { month = (int)_.SmallIntValue, year = (int)_.SmallIntValue, @int = 3 })
					.Union(
						db.Types
							.GroupBy(_ => new { month = _.DateTimeValue.Month, year = _.DateTimeValue.Year })
							.Select(_ => _.Key)
							.Select(_ => new { _.month, _.year, @int = 1 }))
					.Union(
						db.Types.Select(_ => new { month = _.DateTimeValue.Year, year = _.DateTimeValue.Year, @int = 2 })
					)
					.ToList();

				var expected =
					Types.Select(_ => new { month = (int)_.SmallIntValue, year = (int)_.SmallIntValue, @int = 3 })
					.Union(
						Types
							.GroupBy(_ => new { month = _.DateTimeValue.Month, year = _.DateTimeValue.Year })
							.Select(_ => _.Key)
							.Select(_ => new { _.month, _.year, @int = 1 }))
					.Union(
						Types.Select(_ => new { month = _.DateTimeValue.Year, year = _.DateTimeValue.Year, @int = 2 })
					)
					.ToList();

				AreEqual(expected, actual);
			}
		}

		[Table("ConcatTest")]
		[InheritanceMapping(Code = 0, Type = typeof(BaseEntity), IsDefault = true)]
		[InheritanceMapping(Code = 1, Type = typeof(DerivedEntity))]
		class BaseEntity
		{
			[Column]
			public int EntityId { get; set; }
			[Column(IsDiscriminator = true)]
			public int Discr { get; set; }
			[Column]
			public string? Value { get; set; }
		}

		[Table("ConcatTest")]
		class DerivedEntity : BaseEntity
		{
		}

		[Test]
		public void TestConcatInheritance([IncludeDataSources(TestProvName.AllSQLiteClassic)] string context)
		{
			var testData = new[]
			{
				new BaseEntity { Discr = 0, EntityId = 1, Value = "VBase1" },
				new BaseEntity { Discr = 0, EntityId = 2, Value = "VBase2" },
				new BaseEntity { Discr = 0, EntityId = 3, Value = "VBase3" },

				new DerivedEntity { Discr = 1, EntityId = 10, Value = "Derived1" },
				new DerivedEntity { Discr = 1, EntityId = 20, Value = "Derived2" },
				new DerivedEntity { Discr = 1, EntityId = 30, Value = "Derived3" }
			};

			using (var db = GetDataContext(context))
			using (db.CreateLocalTable(testData))
			{
				var result = db.GetTable<BaseEntity>().OfType<BaseEntity>()
					.Concat(db.GetTable<BaseEntity>().OfType<DerivedEntity>())
					.ToArray();

				var expected = testData.Where(t => t.GetType() == typeof(BaseEntity))
					.Concat(testData.OfType<DerivedEntity>())
					.ToArray();

				AreEqualWithComparer(expected, result);
			}

		}

		[ActiveIssue("CI: SQL0418N  The statement was not processed because the statement contains an invalid use of one of the following: an untyped parameter marker, the DEFAULT keyword, or a null", Configuration = ProviderName.DB2)]
		[Test]
		public void TestConcatWithParameterProjection([DataSources] string context)
		{
			using (var db = GetDataContext(context))
			{
				var someValue = 3;
				var items1 = from c in db.Child
					where c.ChildID <= someValue
					select new
					{
						Value = someValue,
						c.ChildID
					};

				var items2 = from c in db.Child
					where c.ChildID > someValue
					select new
					{
						Value = someValue,
						c.ChildID
					};

				var actual = items1.Concat(items2);

				var items1_ = from c in Child
					where c.ChildID <= someValue
					select new
					{
						Value = someValue,
						c.ChildID
					};

				var items2_ = from c in Child
					where c.ChildID > someValue
					select new
					{
						Value = someValue,
						c.ChildID
					};

				var expected = items1_.Concat(items2_);

				AreEqual(expected, actual);
			}
		}

		// https://github.com/linq2db/linq2db/issues/1774
		[Test]
		public void SelectFromUnion([IncludeDataSources(
				true,
				TestProvName.AllOracle,
				TestProvName.AllSqlServer2012Plus,
				TestProvName.AllPostgreSQL)] string context)
		{
			using (var db = GetDataContext(context))
			{
				var q1 = from p in db.Person where p.ID == 1 select new { p.ID };
				var q2 = from p in db.Person where p.ID != 1 select new { p.ID };
				var q = q1.Concat(q2);
				var f = q.Select(t => new { t.ID, rn = Sql.Ext.DenseRank().Over().OrderBy(t.ID).ToValue() }).ToList();
			}
		}

		// https://github.com/linq2db/linq2db/issues/1774
		[Test]
		public void SelectFromUnionReverse([IncludeDataSources(
				true,
				TestProvName.AllOracle,
				TestProvName.AllSqlServer2012Plus,
				TestProvName.AllPostgreSQL)] string context)
		{
			using (var db = GetDataContext(context))
			{
				var q1 = from p in db.Person where p.ID == 1 select new { p.ID };
				var q2 = from p in db.Person where p.ID != 1 select new { p.ID };
				var q = q1.Concat(q2);
				var f = q.Select(t => new { rn = Sql.Ext.DenseRank().Over().OrderBy(t.ID).ToValue(), t.ID }).ToList();
			}
		}

		[Test]
		public void SelectWithNulls([DataSources(TestProvName.AllSybase)] string context)
		{
			using var db = GetDataContext(context);

			var query1 = db.GetTable<LinqDataTypes>();
			var query2 = db.GetTable<LinqDataTypes>().Select(d => new LinqDataTypes { });

			var query = query1.UnionAll(query2);

			query.Invoking(q => q.ToArray()).Should().NotThrow();
		}

		[Test]
		public void SelectWithNulls2([DataSources(TestProvName.AllSybase)] string context)
		{
			using var db = GetDataContext(context);

			var query1 = db.GetTable<LinqDataTypes2>();
			var query2 = db.GetTable<LinqDataTypes2>().Select(d => new LinqDataTypes2 { });

			var query = query1.UnionAll(query2);

			query.Invoking(q => q.ToArray()).Should().NotThrow();
		}

		[Test]
		public void SelectWithBooleanNulls([DataSources] string context)
		{
			using var db = GetDataContext(context);

			var query1 = from x in db.Parent
				select new {a = db.Child.Any(), b = (bool?)(x.ParentID != 0)};

			var query2 = from x in db.Parent
				select new {a = db.Child.Any(), b = (bool?)null};

			var query = query1.UnionAll(query2);

			query.Invoking(q => q.ToList()).Should().NotThrow();
		}		


	}
}
