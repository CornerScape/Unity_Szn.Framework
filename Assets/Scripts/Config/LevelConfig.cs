/*
 * --->SQLite3 dataSyncBase table structure.<---
 * --->This class code is automatically generated。<---
 * --->If you need to modify, please place the custom code between <Self Code Begin> and <Self Code End>.
 *                                                                                    --szn
 */

using Szn.Framework.SQLite3Helper;
using Szn.Framework.Sync;


namespace SQLite3TableDataTmp
{
    public enum LevelConfigEnum
    {
        Id,
        LevelId,
        WordId,
        ChapterId,
        Theme,
    }

    public class LevelConfig : SyncBase
    {
        [SQLite3Constraint(SQLite3Constraint.Unique | SQLite3Constraint.NotNull )]
        [Sync((int)LevelConfigEnum.Id)]
        public int Id { get; private set; }  //序号

        [Sync((int)LevelConfigEnum.LevelId)]
        public int LevelId { get; set; }  //关卡ID

        [Sync((int)LevelConfigEnum.WordId)]
        public int WordId { get; set; }  //词库ID

        [Sync((int)LevelConfigEnum.ChapterId)]
        public int ChapterId { get; set; }  //所属章节

        [Sync((int)LevelConfigEnum.Theme)]
        public string Theme { get; set; }  //主题

        public LevelConfig()
        {
        }

        public LevelConfig(int InId, int InLevelId, int InWordId, int InChapterId, string InTheme)
        {
            Id = InId;
            LevelId = InLevelId;
            WordId = InWordId;
            ChapterId = InChapterId;
            Theme = InTheme;
        }

        //-------------------------------*Self Code Begin*-------------------------------
        //Custom code.
        //-------------------------------*Self Code End*   -------------------------------
        

        public override string ToString()
        {
            return "LevelConfig : Id = " + Id + ", LevelId = " + LevelId + ", WordId = " + WordId + ", ChapterId = " + ChapterId + ", Theme = " + Theme;
        }

    }
}
