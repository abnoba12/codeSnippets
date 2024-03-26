	//- Extension methods used to build selects lists using the @HTML. helper in razor views-
	
	/// <summary>
	/// Builds am HTML select list - By J.Weigand
	/// </summary>
	/// <typeparam name="TModel"></typeparam>
	/// <typeparam name="TProperty"></typeparam>
	/// <param name="htmlHelper"></param>
	/// <param name="expression">The model that this is tied to</param>
	/// <param name="items">An enumerable object to be used in building the HTML options for the select list</param>
	/// <param name="dataValueField">The name of the field from the 'items' parameter you want to use to build the value attribute of each select option</param>
	/// <param name="dataTextField">The name of the field from the 'items' parameter you want to use to build the display text of each select option</param>
	/// <param name="dataDecorators">The name of the field from the 'items' parameter you want to use to build extra HTML5 data attrubites for each select option</param>
	/// <param name="defaultText">If null then no default parameter will be added to the select list. If provided then an default option will be added to the select list with your display text and value of empty string</param>
	/// <param name="htmlAttributes">An object that contains the HTML elemens to set for the element. This applies to the select element and not its child option elements</param>
	/// <returns>Formatted HTML</returns>
	public static MvcHtmlString DropdownListForWithAttributes<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IEnumerable items, string dataValueField, string dataTextField, List<string> dataDecorators = null, string defaultText = null, object htmlAttributes = null)
	{
		//Key is the field name
		//value is the display name
		Dictionary<string, string> dd = new Dictionary<string, string>();
		for (int x = 0; x < dataDecorators.Count(); x++)
		{
			dd.Add(dataDecorators.ElementAt(x), dataDecorators.ElementAt(x));
		}
		return DropdownListForWithAttributes(htmlHelper, expression, items, dataValueField, dataTextField, dd, defaultText, htmlAttributes);
	}
	
	/// <summary>
	/// Builds am HTML select list - By J.Weigand
	/// </summary>
	/// <typeparam name="TModel"></typeparam>
	/// <typeparam name="TProperty"></typeparam>
	/// <param name="htmlHelper"></param>
	/// <param name="expression">The model that this is tied to</param>
	/// <param name="items">An enumerable object to be used in building the HTML options for the select list</param>
	/// <param name="dataValueField">The name of the field from the 'items' parameter you want to use to build the value attribute of each select option</param>
	/// <param name="dataTextField">The name of the field from the 'items' parameter you want to use to build the display text of each select option</param>
	/// <param name="dataDecorators">A dictonary of the fields from the 'items' parameter with custom names that you want to use to build extra HTML5 data attrubites for each select option. The dictonary key needs to be the name of the feild in the 'items' parameter and the value needs to be what you want to name your custom attribute</param>
	/// <param name="defaultText">If null then no default parameter will be added to the select list. If provided then an default option will be added to the select list with your display text and value of empty string</param>
	/// <param name="htmlAttributes">An object that contains the HTML elemens to set for the element. This applies to the select element and not its child option elements</param>
	/// <returns>Formatted HTML</returns>
	public static MvcHtmlString DropdownListForWithAttributes<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IEnumerable items, string dataValueField, string dataTextField, IDictionary<string, string> dataDecorators = null, string defaultText = null, object htmlAttributes = null)
	{
		if (expression == null)
		{
			throw new ArgumentNullException("expression");
		}
		ModelMetadata metadata = ModelMetadata.FromLambdaExpression<TModel, TProperty>(expression, htmlHelper.ViewData);
		string name = ExpressionHelper.GetExpressionText((LambdaExpression)expression);
		return DropdownListWithAttributes(htmlHelper, name, items, dataValueField, dataTextField, dataDecorators, defaultText, HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes), metadata);
	}

	/// <summary>
	/// Builds am HTML select list - By J.Weigand
	/// </summary>
	/// <param name="htmlHelper"></param>
	/// <param name="name">The name and id html attribute for the select list</param>
	/// <param name="items">An enumerable object to be used in building the HTML options for the select list</param>
	/// <param name="dataValueField">The name of the field from the 'items' parameter you want to use to build the value attribute of each select option</param>
	/// <param name="dataTextField">The name of the field from the 'items' parameter you want to use to build the display text of each select option</param>
	/// <param name="dataDecorators">The name of the field from the 'items' parameter you want to use to build extra HTML5 data attrubites for each select option</param>
	/// <param name="defaultText">If null then no default parameter will be added to the select list. If provided then an default option will be added to the select list with your display text and value of empty string</param>
	/// <param name="htmlAttributes">An object that contains the HTML elemens to set for the element. This applies to the select element and not its child option elements</param>
	/// <param name="metadata">Parameter used internally</param>
	/// <returns>Formatted HTML</returns>
	public static MvcHtmlString DropdownListWithAttributes(this HtmlHelper htmlHelper, string name, IEnumerable items, string dataValueField, string dataTextField, List<string> dataDecorators = null, string defaultText = null, IDictionary<string, object> htmlAttributes = null, ModelMetadata metadata = null)
	{
		//Key is the field name
		//value is the display name
		Dictionary<string, string> dd = new Dictionary<string, string>();
		for (int x = 0; x < dataDecorators.Count(); x++)
		{
			dd.Add(dataDecorators.ElementAt(x), dataDecorators.ElementAt(x));
		}
		return DropdownListWithAttributes(htmlHelper, name, items, dataValueField, dataTextField, dd, defaultText, HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes), metadata);
	}

	/// <summary>
	/// Builds am HTML select list - By J.Weigand
	/// </summary>
	/// <param name="htmlHelper"></param>
	/// <param name="name">The name and id html attribute for the select list</param>
	/// <param name="items">An enumerable object to be used in building the HTML options for the select list</param>
	/// <param name="dataValueField">The name of the field from the 'items' parameter you want to use to build the value attribute of each select option</param>
	/// <param name="dataTextField">The name of the field from the 'items' parameter you want to use to build the display text of each select option</param>
	/// <param name="dataDecorators">A dictonary of the fields from the 'items' parameter with custom names that you want to use to build extra HTML5 data attrubites for each select option. The dictonary key needs to be the name of the feild in the 'items' parameter and the value needs to be what you want to name your custom attribute</param>
	/// <param name="defaultText">If null then no default parameter will be added to the select list. If provided then an default option will be added to the select list with your display text and value of empty string</param>
	/// <param name="htmlAttributes">An object that contains the HTML elemens to set for the element. This applies to the select element and not its child option elements</param>
	/// <param name="metadata">Parameter used internally</param>
	/// <returns>Formatted HTML</returns>
	public static MvcHtmlString DropdownListWithAttributes(this HtmlHelper htmlHelper, string name, IEnumerable items, string dataValueField, string dataTextField, IDictionary<string, string> dataDecorators = null, string defaultText = null, IDictionary<string, object> htmlAttributes = null, ModelMetadata metadata = null)
	{
		string fullName = htmlHelper.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(name);
		if (String.IsNullOrEmpty(fullName))
		{
			throw new ArgumentException("name");
		}

		TagBuilder dropdown = new TagBuilder("select");
		dropdown.Attributes.Add("name", fullName);
		dropdown.GenerateId(fullName);
		dropdown.MergeAttributes(htmlAttributes);
		dropdown.MergeAttributes(htmlHelper.GetUnobtrusiveValidationAttributes(name, metadata));

		StringBuilder options = new StringBuilder();

		// Make optionLabel the first item that gets rendered.
		if (defaultText != null)
			options = options.Append("<option value='" + String.Empty + "'>" + defaultText + "</option>");

		foreach (object item in items)
		{
			var itemDataValueField = item.GetType().GetProperty(dataValueField).GetValue(item, null);
			var itemDataTextField = item.GetType().GetProperty(dataTextField).GetValue(item, null);

			options = options.Append("<option value='" + itemDataValueField + "' ");
			if (itemDataValueField != null && metadata != null && metadata.Model != null && itemDataValueField.ToString() == metadata.Model.ToString())
			{
				options = options.Append("selected='selected'");
			}
			if (dataDecorators != null)
			{
				//Key is the field name
				//value is the display name
				foreach (KeyValuePair<string, string> dataAttr in dataDecorators)
				{
					var itemDataAttr = item.GetType().GetProperty(dataAttr.Key).GetValue(item, null);

					//Remove specail chars
					Regex rgx = new Regex("[^a-zA-Z0-9-]");
					string formattedDataAttr = (String.IsNullOrEmpty(dataAttr.Value))? rgx.Replace(dataAttr.Key, "-"):rgx.Replace(dataAttr.Value, "-");

					options = options.Append("data-" + formattedDataAttr + "='" + itemDataAttr + "' ");
				}
			}
			options = options.Append(">" + itemDataTextField + "</option>");
		}
		dropdown.InnerHtml = options.ToString();
		return MvcHtmlString.Create(dropdown.ToString(TagRenderMode.Normal));
	}