using System.IO;
using FluentValidation;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using FluentValidation.Attributes;
using HandBook.Application.Helpers;
using Microsoft.AspNetCore.Hosting;
using HandBook.Application.Infrastructure;

namespace HandBook.Application.Commands.Photo
{
    [Validator(typeof(PlacePhotoCommandValidator))]
    public class PlacePhotoCommand : Command
    {
        public IFormFile File { get; set; }

        public async override Task<CommandExecutionResult> ExecuteAsync()
        {
            var hostingEnvironment = GetService<IHostingEnvironment>();
            var path = Path.Combine(hostingEnvironment.ContentRootPath, "Source/Images");

            var filePath = Path.Combine(path, File.FileName);

            using (Stream fileStream = new FileStream(filePath, FileMode.Create))
            {
                File.CopyTo(fileStream);

                var image = SixLabors.ImageSharp.Image.Load(File.OpenReadStream());

                var photo = new Domain.PhotoManagement.Photo(image.Width,
                                                             image.Height,
                                                             filePath);

                await SaveAsync(photo, _photoRepository);
            }

            return await OkAsync(new DomainOperationResult());
        }
    }

    internal class PlacePhotoCommandValidator : AbstractValidator<PlacePhotoCommand>
    {
        public PlacePhotoCommandValidator()
        {
            RuleFor(photo => photo.File)
                .NotEmpty()
                .NotNull()
                .WithMessage("Please, upload photo.");

            RuleFor(photo => photo.File)
                .SetValidator(new FormFileValidation());
        }
    }
}
