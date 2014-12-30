/* Copyright (c) 2008-2012 Peter Palotas
 *  
 *  Permission is hereby granted, free of charge, to any person obtaining a copy
 *  of this software and associated documentation files (the "Software"), to deal
 *  in the Software without restriction, including without limitation the rights
 *  to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 *  copies of the Software, and to permit persons to whom the Software is
 *  furnished to do so, subject to the following conditions:
 *  
 *  The above copyright notice and this permission notice shall be included in
 *  all copies or substantial portions of the Software.
 *  
 *  THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 *  IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 *  FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 *  AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 *  LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 *  OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 *  THE SOFTWARE.
 */
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Collections.ObjectModel;

namespace AlphaShadow
{
   public abstract class Command
   {
      #region Private Fields

      private Dictionary<string, IList<string>> m_optionValues = new Dictionary<string, IList<string>>(StringComparer.OrdinalIgnoreCase);
      private List<string> m_remainingArguments = new List<string>();
      private static Regex s_argumentRegex = new Regex(
         "^(?(/|-) \r\n(/|-)\r\n(?<name>[a-zA-Z0-9_]+)\r\n(\r\n(:|=)\r\n(?<value" +
         ">.*)\r\n)?\r\n|\r\n(?<value>.*)\r\n)$",
          RegexOptions.IgnoreCase
          | RegexOptions.Multiline
          | RegexOptions.ExplicitCapture
          | RegexOptions.CultureInvariant
          | RegexOptions.IgnorePatternWhitespace
          | RegexOptions.Compiled
      );
      

      #endregion

      #region Constructor

      public Command(string commandName, string description)
      {
         if (String.IsNullOrEmpty(commandName))
            throw new ArgumentException("commandName is null or empty.", "commandName");

         if (String.IsNullOrEmpty(description))
            throw new ArgumentException("description is null or empty.", "description");

         Name = commandName;
         Description = description;
      }

      #endregion

      #region Properties

      public string Name { get; private set; }
      public string Description { get; private set; }
      public IUIHost Host { get; private set; }
      public abstract IEnumerable<OptionSpec> Options { get; }
      protected IList<string> RemainingArguments
      {
         get
         {
            return new ReadOnlyCollection<string>(m_remainingArguments);
         }
      }
      
      public OptionSpec UnnamedOption
      {
         get
         {
            return Options.SingleOrDefault(opt => opt.Name.Length == 0);
         }
      }

      public IEnumerable<OptionSpec> NamedOptions
      {
         get
         {
            return Options.Where(opt => opt.Name.Length > 0);
         }
      }
      #endregion

      #region Protected Methods

      protected T GetOptionValue<T>(OptionSpec option)
      {
         TypeConverter converter = TypeDescriptor.GetConverter(typeof(T));
         IList<string> values;
         
         if (m_optionValues.TryGetValue(option.Name, out values))
         {
            if (values.Count > 1)
               throw new InvalidOperationException("GetOptionValue cannot be used to retreive multiple values.");

            if (values.Count == 1)
            {
               try
               {
                  return (T)converter.ConvertFromInvariantString(values.Single());
               }
               catch (Exception ex)
               {
                  throw new FormatException(String.Format("\"{0}\" is not a valid value for option /{1}: {2}", values.Single(), option, ex.Message));
               }
            }
         }

         throw new KeyNotFoundException(String.Format("Option {0} not specified.", option.Name));
      }

      protected IList<T> GetOptionValues<T>(OptionSpec option)
      {
         List<T> result = new List<T>();
         TypeConverter converter = TypeDescriptor.GetConverter(typeof(T));
         IList<string> values;
         if (m_optionValues.TryGetValue(option.Name, out values))
         {
            foreach (string value in values.Where(val => val != null))
            {
               result.Add((T)converter.ConvertFromInvariantString(value));
            }
         }
         return result;
      }

      protected bool HasOption(OptionSpec option)
      {
         return m_optionValues.ContainsKey(option.Name);
      }

      protected bool HasValue(OptionSpec option)
      {
         IList<string> values;
         if (!m_optionValues.TryGetValue(option.Name, out values))
            return false;

         return values.Any(val => val != null);
      }

      protected abstract void ProcessOptions();

      #endregion

      #region Public Methods

      public abstract void Run();

      public virtual void Initialize(IUIHost host, IEnumerable<string> args)
      {
         if (host == null)
            throw new ArgumentNullException("log", "log is null.");

         Host = host;
         

         foreach (string argument in args)
         {
            Match match = s_argumentRegex.Match(argument);
            string optionName = null;
            string value = null;

            if (match.Success)
            {
               if (match.Groups["name"].Success)
               {
                  optionName = match.Groups["name"].Value;
               }

               if (match.Groups["value"].Success)
               {
                  value = match.Groups["value"].Value;
               }

               if (optionName == null)
               {
                  if (UnnamedOption == null || ((UnnamedOption.ValueType & OptionType.MultipleValuesAllowed) == 0) && m_remainingArguments.Any())
                     throw new ArgumentException(String.Format("Unrecognized argument \"{0}\".", value));

                  m_remainingArguments.Add(value);
               }
               else
               {
                  if (!Options.Any(opt => opt.Name.Equals(optionName, StringComparison.OrdinalIgnoreCase)))
                     throw new ArgumentException(String.Format("Unrecognized option /{0}.", optionName));

                  IList<string> values;
                  if (!m_optionValues.TryGetValue(optionName, out values))
                  {
                     values = new List<string>();
                     m_optionValues.Add(optionName, values);
                  }
                     
                  values.Add(value);
               }
            }
            else
            {
               throw new ArgumentException(String.Format("Unrecognized option \"{0}\"", argument));
            }
         }

         foreach (OptionSpec option in NamedOptions)
         {
            IList<string> values;
            bool hasOption = m_optionValues.TryGetValue(option.Name, out values);

            if (option.IsRequired && !hasOption)
               throw new ArgumentException(String.Format("Missing required option /{0}.", option.Name));

            if (hasOption && (option.ValueType & OptionType.Required) != 0 && (values.Any(val => val == null) || values.Count == 0))
               throw new ArgumentException(String.Format("Missing required value for option /{0}", option.Name));

            if (hasOption && option.ValueType == OptionType.ValueProhibited && values.Any(val => val != null))
               throw new ArgumentException(String.Format("Option /{0} does not accept a value.", option.Name));

            if (hasOption && (option.ValueType & OptionType.MultipleValuesAllowed) == 0 && values.Count(val => val != null) > 1)
               throw new ArgumentException(String.Format("Option /{0} must only be specified once.", option.Name));
         }

         if (UnnamedOption != null)
         {
            if (UnnamedOption.IsRequired && RemainingArguments.Count == 0)
               throw new ArgumentException(String.Format("Missing required argument(s) \"{0}\".", UnnamedOption.ValueText));
         }

         ProcessOptions();

      }

      #endregion
   }
}