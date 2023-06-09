﻿using FamilyBudget.DAL;
using FamilyBudget.DAL.Data;
using FamilyBudget.WebApp.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Reflection;

namespace FamilyBudget.WebApp.Pages.Expenses;

[Authorize]
public class IndexModel : PageModel
{
    private readonly FamilyBudgetDbContext _context;

    public IndexModel(
        FamilyBudgetDbContext context)
    {
        _context = context;
    }

    public PaginatedList<Expense> Expenses { get; set; } = default!;

    public async Task OnGetAsync(string sortOrder,
        string searchString,
        int? pageNumber,
        string searchUser,
        CancellationToken cancellationToken)
    {
        if (_context.Expenses is null)
        {
            return;
        }

        var sharingUsers = await _context.UserExpenseSharingOptions
                .Where(x => x.SharedWithUser.UserName == User.Identity.Name)
                .Select(x => x.FamilyBudgetUser.UserName)
                .ToArrayAsync();

        ViewData["SharingUsers"] = new SelectList(sharingUsers);

        PrepareAllOrderParams(sortOrder);
        ViewData["CurrentSort"] = sortOrder;
        var currentFilter = ViewData["CurrentFilter"]?.ToString();
        ViewData["CurrentFilter"] = searchString;

        var currentSearchUser = ViewData["CurrentSearchUser"]?.ToString();
        ViewData["CurrentSearchUser"] = searchUser;

        if (searchString != currentFilter || currentSearchUser != searchUser)
        {
            pageNumber = 1;
        }
        else
        {
            searchString = currentFilter;
        }

        var allowedUsersIds = await _context.UserExpenseSharingOptions
            .Where(x => x.SharedWithUser.UserName == User.Identity.Name)
            .Select(x => x.FamilyBudgetUserId)
            .ToArrayAsync();

        var query = _context.Expenses
                .Where(x => allowedUsersIds.Contains(x.FamilyBudgetUserId.Value)
                    || x.FamilyBudgetUser.UserName == User.Identity.Name);

        query = PrepareOrderedQuery(query, sortOrder);

        if (!string.IsNullOrEmpty(searchString))
        {
            query = query.Where(x => x.Title.Contains(searchString)
                || x.Description.Contains(searchString));
        }

        if (!string.IsNullOrEmpty(searchUser))
        {
            query = query.Where(x => x.FamilyBudgetUser.UserName == searchUser);
        }

        const int pageSize = 5;
        Expenses = await PaginatedList<Expense>.CreateAsync(
            query.Include(x => x.FamilyBudgetUser),
            pageNumber ?? 1,
            pageSize);
    }

    private static IQueryable<Expense> PrepareOrderedQuery(
        IQueryable<Expense> query,
        string sortOrder)
    {
        // This could be chained.
        query = OrderFromProperty(query, x => x.Title, sortOrder);
        query = OrderFromProperty(query, x => x.Description, sortOrder);
        query = OrderFromProperty(query, x => x.AmountSpent, sortOrder);
        query = OrderFromProperty(query, x => x.Category, sortOrder);
        // with current approach won't work
        //query = OrderFromProperty(query, x => x.FamilyBudgetUser.UserName, sortOrder);
        return query;
    }

    private static IQueryable<Expense> OrderFromProperty<T>(
        IQueryable<Expense> query,
        Expression<Func<Expense, T>> lambda,
        string sortOrder)
    {
        if (ShouldBeOrderedBy(lambda, sortOrder))
        {
            return IsAscending(sortOrder)
                ? query.OrderBy(lambda)
                : query.OrderByDescending(lambda);
        }

        return query;
    }

    private static bool ShouldBeOrderedBy<T, TKey>(Expression<Func<T, TKey>> lambda, string sortOrder)
    {
        var propInfo = (lambda.Body as MemberExpression).Member as PropertyInfo;
        return sortOrder?.StartsWith(propInfo.Name + "_", StringComparison.OrdinalIgnoreCase)
            ?? false;
    }

    private static bool IsAscending(string sortOrder)
    {
        return sortOrder.EndsWith("_asc", StringComparison.OrdinalIgnoreCase);
    }

    private void PrepareAllOrderParams(string sortOrder)
    {
        PrepareOrderParam(x => x.Title, sortOrder);
        PrepareOrderParam(x => x.Description, sortOrder);
        PrepareOrderParam(x => x.AmountSpent, sortOrder);
        PrepareOrderParam(x => x.Category, sortOrder);
    }

    private void PrepareOrderParam<T>(
        Expression<Func<Expense, T>> lambda,
        string sortOrder)
    {
        var propName = ((lambda.Body as MemberExpression).Member as PropertyInfo).Name;

        var ascOrder = $"{propName}_asc".ToLower();
        var descOrder = $"{propName}_desc".ToLower();

        ViewData[$"{propName}SortParam"] = sortOrder == descOrder
            ? descOrder
            : ascOrder;
    }
}
