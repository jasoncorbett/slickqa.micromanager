/*
 * Created by SharpDevelop.
 * User: jcorbett
 * Date: 11/10/2011
 * Time: 2:42 PM
 * 
 */
using System;
using System.IdentityModel.Selectors;
using System.IdentityModel.Tokens;

namespace MicroManager
{
	/// <summary>
	/// Description of CustomUsernamePasswordValidator.
	/// </summary>
	public class CustomUsernamePasswordValidator : UserNamePasswordValidator
	{
		public CustomUsernamePasswordValidator()
		{
		}
		
		public override void Validate(string userName, string password)
		{
			if(!"mmadmin".Equals(userName) || !"f00b@r".Equals(password))
				throw new SecurityTokenValidationException("Username or Password unrecognized");
		}
	}
}
