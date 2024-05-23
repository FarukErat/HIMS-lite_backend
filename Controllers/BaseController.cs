using System.Web.Http.Cors;
using Microsoft.AspNetCore.Mvc;

namespace Controllers;

[ApiController]
[Route("[controller]")]
[EnableCors(origins: "*", headers: "*", methods: "*")]
public class BaseController : ControllerBase;
