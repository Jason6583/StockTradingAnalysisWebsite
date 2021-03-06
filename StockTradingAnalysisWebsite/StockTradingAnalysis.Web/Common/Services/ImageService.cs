﻿using System;
using System.IO;
using System.Web;
using Microsoft.AspNetCore.Http;
using StockTradingAnalysis.Core.Types;
using StockTradingAnalysis.Interfaces.Domain;
using StockTradingAnalysis.Interfaces.Services.Core;
using StockTradingAnalysis.Web.Common.Interfaces;

namespace StockTradingAnalysis.Web.Common.Services
{
	/// <summary>
	/// The class ImageService defines an service which can analyse images
	/// </summary>
	public class ImageService : IImageService
	{
		/// <summary>
		/// Returns the image as byte array and meta information
		/// </summary>
		/// <param name="file">The uploaded image</param>
		/// <param name="description">The description</param>
		/// <param name="id">The id of the image</param>
		/// <returns><c>True</c>if an image was found</returns>
		public IImage GetImage(IFormFile file, string description, Guid id)
		{
			var image = new Image(id);

			if (file == null || file.Length == 0)
				return null;

			image.OriginalName = file.FileName;
			image.ContentType = file.ContentType;
			image.Description = description;

			using (var ms = new MemoryStream())
			{
				file.CopyTo(ms);
				image.Data = ms.ToArray();
			}

			return image;
		}

		/// <summary>
		/// Returns the image as byte array and meta information
		/// </summary>
		/// <param name="contentType">The content type</param>
		/// <param name="description">The description</param>
		/// <param name="id">The id of the image</param>
		/// <param name="data">The byte array</param>
		/// <param name="originalName">The original name</param>
		/// <returns><c>True</c>if an image was found</returns>
		public IImage GetImage(byte[] data, string originalName, string contentType, string description, Guid id)
		{
			var image = new Image(id);

			if (data == null || data.Length == 0)
				return null;

			image.Description = description;
			image.ContentType = contentType;
			image.Data = data;
			image.OriginalName = originalName;

			return image;
		}
	}
}