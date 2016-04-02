using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SExpressionParser
{
    /// <summary>
    /// 文字列をS式としてパースするクラス
    /// </summary>
    public class Parser
    {
        // 現在処理対象となっている文字の文字列中のインデックス
        private int _idx;

        /// <summary>
        /// S式としてパースする
        /// </summary>
        /// <param name="content">パースしたい文字列</param>
        /// <returns>パース結果</returns>
        public dynamic Parse(string content)
        {
            dynamic result = null;

            try
            {
                // 1文字ずつ読み込んでバッファに貯めこむ
                for (; _idx < content.Length; _idx++)
                {
                    // '('が来たらリストとして処理
                    if (content[_idx] == '(')
                    {
                        result = ParseList(content);
                        break;
                    }
                    // 数値か'.'・'+'・'-'が来たら数値としてパース
                    else if (Char.IsDigit(content[_idx]) || content[_idx] == '.' || content[_idx] == '+' ||
                             content[_idx] == '-')
                    {
                        result = ParseNumber(content, false);
                        break;
                    }
                    // 2重引用符がきたら文字列としてパース
                    else if (content[_idx] == '"')
                    {
                        result = ParseString(content, false);
                        break;
                    }
                }
            }
            catch (Exception)
            {
                // 何らかのエラーがあったら不正な文字列が渡されたとして例外を投げる
                throw new ParseException();
            }

            // パースした後に残った文字列に空白文字以外が含まれていたらエラー
            if (_idx < content.Length)
                if (content.Substring(_idx).Any(c => !Char.IsWhiteSpace(c)))
                    throw new ParseException("Invalid element");

            return result;
        }

        /// <summary>
        /// リストをパースする
        /// </summary>
        /// <param name="content">パース対象</param>
        /// <returns>パースしたリスト</returns>
        private List<dynamic> ParseList(string content)
        {
            var list = new List<dynamic>();
            var finish = false;

            // リストの始まりである')'が先に読み込まれているのでその分だけインデックスをインクリメント
            _idx++;

            // 1文字ずつ読み込んでバッファに貯めこむ
            for (; _idx < content.Length; _idx++)
            {
                // '('が来たらリストが中に含まれているので再帰
                if (content[_idx] == '(')
                    list.Add(ParseList(content));
                // 数値か'.'・'+'・'-'が来たら数値としてパース
                if (Char.IsDigit(content[_idx]) || content[_idx] == '.' || content[_idx] == '+' || content[_idx] == '-')
                    list.Add(ParseNumber(content, true));
                // 2重引用符がきたら文字列としてパース
                if (content[_idx] == '"')
                    list.Add(ParseString(content, true));
                // '('が来たらリストの終端であることを表しているので')'のぶんだけインデックスをインクリメントして終了
                if (content[_idx] == ')')
                {
                    _idx++;
                    finish = true;
                    break;
                }
            }

            // この時点で')'が着ていないのにこれ以上パースする文字がなければ不正なS式が渡されたとする
            if (_idx == content.Length && !finish)
                throw new ParseException("Invalid list");

            return list;
        }

        /// <summary>
        /// S式中の数値をパースする
        /// </summary>
        /// <param name="content">パース対象</param>
        /// <param name="inList">パースしたい数値がリスト中にあるかどうか</param>
        /// <returns>パースした数値</returns>
        private dynamic ParseNumber(string content, bool inList)
        {
            var buffer = new StringBuilder(content[_idx++].ToString());

            // 1文字ずつ読み込んでバッファに貯めこむ
            for (; _idx < content.Length; _idx++)
            {
                // リスト中なら空白か')'が来たら終了
                if (inList && (Char.IsWhiteSpace(content[_idx]) || content[_idx] == ')'))
                    break;
                // リスト中でないなら空白が来たら終了
                if (Char.IsWhiteSpace(content[_idx]))
                    break;
                
                buffer.Append(content[_idx]);
            }

            var number = buffer.ToString();
            
            // '.'が含まれているなら浮動小数
            // そうでないなら整数
            if (number.Contains('.'))
                return double.Parse(buffer.ToString());
            return int.Parse(number);
        }
        
        /// <summary>
        /// S式中の文字列をパースする
        /// </summary>
        /// <param name="content">パース対象</param>
        /// <param name="inList">パースしたい文字列がリスト中にあるかどうか</param>
        /// <returns>パースした文字列</returns>
        private string ParseString(string content, bool inList)
        {
            var buffer = new StringBuilder();
            var end = true;

            // 先に2重引用符が読み込まれているのでその分だけインデックスをインクリメント
            _idx++;

            // 1文字ずつ読み込んでバッファに貯めこむ
            for (; _idx < content.Length; _idx++)
            {
                // 2重引用符はエスケープして表現される
                if (content[_idx] == '\\')
                    end = false;
                // エスケープされていない2重引用符が来たら文字列の終端を表している
                if (content[_idx] == '"')
                    if (end)
                        break;
                    else
                        end = true;
                if (end)
                    buffer.Append(content[_idx]);
            }

            // リスト中であればパースされた文字列の後に')'か空白文字が着ていない場合は不正な文字とする
            // 例: "123"3
            if (inList && !(Char.IsWhiteSpace(content[_idx + 1]) || content[_idx + 1] == ')'))
                throw new ParseException("Invalid string");

            // 文字列終端を表している2重引用符分インデックスをインクリメント
            _idx++;

            return buffer.ToString();
        }
    }
}
