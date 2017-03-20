using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web.Mvc;

namespace ManagementApplication
{
	public class MetadataProvider : DataAnnotationsModelMetadataProvider
	{
		public MetadataProvider()
		{
		}

		protected override ModelMetadata CreateMetadata(IEnumerable<Attribute> attributes, Type containerType, Func<object> modelAccessor, Type modelType, string propertyName)
		{
			ModelMetadata metadata = base.CreateMetadata(attributes, containerType, modelAccessor, modelType, propertyName);
			DescriptionAttribute additionalValues = attributes.OfType<DescriptionAttribute>().FirstOrDefault<DescriptionAttribute>();
			metadata.AdditionalValues.Add("Attributes", attributes);
			if (additionalValues != null)
			{
				metadata.Description = additionalValues.Description;
			}
			return metadata;
		}
	}
}